---
title: Git Release Notes
date: "2021-03-31"
description: "Adding Simple Git Release Notes to Your CI"
---

The problem often arises in software engineer to capture release notes for your end users.  The situation quickly complicates because the notion of "end users" changes per environment.  The developers may be using a DEV environment while the QA may be using another.  And, then you may need another UAT environment with the primary stake holders.  Each of these environments should have a some form of documentation for the release.  

Enter `git` to the rescue.  I typically follow the 'git flow' workflow.  Also, over time I have begun to use `rebase` with `squash` instead of a simple `merge` to keep the commit log clean.  With this workflow, it is very easy to setup a simple CI job that logs the changes from git since the last commit.  

The script below hinges on the following git command:

```
git diff --name-only HEAD~0 HEAD~100
```

Here's a `pwsh` script to get you started.  Enjoy!


```powershell
### Add the Release Notes from Git ###
$releaseNotes = "# Release Notes`n`n"


# Add the commit message and hash title.
$commitMessage = git log -1 --pretty='%s'
$commitHash = git log -1 --pretty='%H'
$commitUrl = git remote get-url origin | Foreach-Object { $_ -replace "\.git", "/-/commit/$commitHash" }
$releaseNotes += "### [$commitMessage]($commitUrl)`n`n"


$date = (Get-Date).ToString()
$releaseNotes += "> $date`n`n"


# Get the file changes in the last commit
git diff --name-only HEAD~0 HEAD~100 |
ForEach-Object {
    $title = $_
    $exists = $False

    # Test if the path is still in source.
    if (Test-Path($_)) {
        # If it is, then display a ➕ to indicate the file changed.
        $title = "➕ $title"
        $exists = $True
    }
    else {
        # Otherwise, display a ➖ to indicate the file was removed.
        $title = "➖ ~~$title~~"
    }
    $releaseNotes += "- $title`n"
}


$releaseNotes += "`n`n"

# Prepend the new release notes with the content from the previous index.
$releaseNotes = @($releaseNotes) + (Get-Content index.md | Select-Object -Skip 1)


# Finally, save the changes.
$releaseNotes | Set-Content index.md
```