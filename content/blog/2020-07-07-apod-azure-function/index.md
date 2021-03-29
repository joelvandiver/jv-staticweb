---
title: APOD Azure Function
date: "2020-07-07"
description: "Create an Azure function to save the Astronomy Photo of the Day into a storage account."
---

I've written about setting up a [JS photo script](./2019-09-04-nasa-apod.html) with NASA Astronomy Photo of the Day (APOD).  But, to be honest, I've gotten a little greedy with these photos.  I like that I can go to my blog and get the photo of the day, but I would prefer to keep a cache of previous photos of the day!  

## Enter Azure Functions

I already store my blog on an Azure storage account.  Wouldn't it be great if I could automatically get the APOD photo and put it into that storage account?

Since I wrote the original script in JavaScript, I opted for TypeScript as the Azure Function language of choice.  

After pulling down an auto-generated Azure Function into VS Code, I did the following.

First, I setup the dependencies:

> package.json:

```json
  "dependencies": {
    "@azure/storage-blob": "^12.2.0-preview.1",
    "node-fetch": "^2.6.0"
  },
  "devDependencies": {
    "@azure/functions": "^1.0.2-beta2",
    "@types/node-fetch": "^2.5.7",
    "typescript": "^3.3.3"
  }
```

Second, I defined the script.  

```typescript
import { AzureFunction, Context } from "@azure/functions"
import { ContainerClient, StorageSharedKeyCredential } from "@azure/storage-blob";
import fetch from "node-fetch";

const getFileName = function (url: string) {
    const s = url.split("/");
    const l = s.length;
    return s[l - 1];
};

const saveImg = async function (url: string, context: Context) {
    var timeStamp = new Date().toISOString();
    context.log('Saving url:  ' + url, timeStamp);
    const creds = new StorageSharedKeyCredential("{STORAGE_ACCOUNT_NAME}", "{STORAGE_ACCOUNT_KEY}");
    const client = new ContainerClient("{STORAGE_ACCOUNT_URI}/assets/img/apod", creds)
    const filename = getFileName(url);
    const blob = client.getBlobClient(filename);
    await blob.syncCopyFromURL(url);
};

const sync = async function (context: Context) {
    const json_url = "https://api.nasa.gov/planetary/apod?api_key={APOD_API_KEY}";
    context.log("Fetching " + json_url);
    await fetch(json_url)
        .then(function (response) { return response.json(); })
        .then(function (json) {
            const timeStamp = new Date().toISOString();
            const img_url = json.url;
            if (json.media_type !== "image") {
                context.log('The json response is not an image: ' + img_url, timeStamp);
                return;
            }
            context.log("Fetched " + json_url);
            return saveImg(img_url, context);
        });
};

const timerTrigger: AzureFunction = async function (context: Context, myTimer: any): Promise<void> {
    const timeStamp = new Date().toISOString();
    context.log('Syncing', timeStamp);
    await sync(context);
};

export default timerTrigger;
```

Third, I set the function to run on a timer since this is the "Photo of the Day".  

That's it!  Now, I have a simple Azure Function running in the cloud, storing the photos in the cloud, of the astronomy beyond the clouds!  Nice!