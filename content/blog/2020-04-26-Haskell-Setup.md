---
title: Haskell Setup
date: "2020-04-26"
description: "Journey towards functional purity"
---

In my journey down the path of *purity* in functional programming, I have inevitably merged with the path of `Haskell` programming.  `Haskell` seems like an amazing language.  I started my career rooted in Mathematics, and I feel at home coding in the `Haskell` programming language.  

But, I've also seen the pain of setting up my dev system for multiple types of coding environments.  For quick console coding, I'd really like to get off the train of managing installs on my system.  

## Docker

Let's setup a `Haskell` dev environment with [Docker](https://www.docker.com/) to isolate the environment.

**Requirements**

1. The `Haskell` coding environment should be isolated from the dev system.
2. The *.hs code files should be reloaded without having to rebuild Docker images.

> PowerShell

```powershell
# ${PWD} is the current directory in PowerShell
docker run -it --rm -v ${PWD}:/app haskell:8

:load app/main.hs
```

Let's get to `Haskell` code!