---
title: Git Release Notes
date: "2021-03-31"
description: "Adding Simple Git Release Notes to Your CI"
---

The problem often arises in software engineer to capture release notes for your end users.  The situation quickly complicates because the notion of "end users" changes per environment.  The developers may be using a DEV environment while the QA may be using another.  And, then you may need another UAT environment with the primary stake holders.  Each of these environments should have a some form of documentation for the release.  

Enter `git` to the rescue.  I typically follow the 'git flow' workflow.  Also, over time I have begun to use `rebase` with `squash` instead of a simple `merge` to keep the commit log clean.  With this workflow, it is very easy to setup a simple CI job that logs the changes from git since the last commit.  

The script below hinges on the following git command:

```
git diff --name-only HEAD~0 HEAD~1
```

Here's a `pwsh` script to get you started.  Enjoy!


```powershell
Write-Host "Running Release Notes" -ForegroundColor Yellow


Function Save-CommitNotes([int]$commitIndex) {
    
    # Add the Release Notes
    $releaseNotes = "# Release Notes`n`n"
    $commits = git log -200 --pretty='%H'
    $commitHash = $commits[$commitIndex]
    $commitTitle = git log -1 --pretty='%s' $commitHash
    $date = git show --no-patch --no-notes --pretty='%cd' $commitHash
    
    Write-Host "Logging $commitHash from $date at index $commitIndex." -ForegroundColor Yellow


    # Add the commit message and hash title.
    $commitUrl = git remote get-url origin | Foreach-Object { $_ -replace "\.git", "/-/commit/$commitHash" }
    $releaseNotes += "### [$commitTitle]($commitUrl)`n`n"


    $releaseNotes += "> $date`n`n"


    # Get the file changes in the last commit
    $nextCommitIndex = $commitIndex + 1
    # https://git-scm.com/docs/git-diff#Documentation/git-diff.txt---diff-filterACDMRTUXB82308203
    git diff HEAD~$nextCommitIndex HEAD~$commitIndex --name-status |
    ForEach-Object {
        $line = $_ -replace "`t", " "
        # Remove the Status code and leading space
        #   Example:  'M       .gitignore'
        $title = $line.Substring(1, $line.Length - 1).Trim()
        $status = $line[0]


        switch($status) {
            # Added
            "A" { $title = "➕ $title" }
            # Modified
            "M" { $title = "🖊 $title" }
            # Renamed
            "R" { $title = "↪ $title" }
            # Deleted
            "D" { $title = "➖ ~~$title~~" }
            # Other
            default { $title = "($status) $title"}
        }
        
        if ($line.EndsWith(".md") -and ($status -ne "D")) {
            # Convert the markdown file changes to links.
            # Set the url as the last segment after the space
            $split = $line.Split(" ")
            $url = $split[$split.Length - 1]
            $releaseNotes += "- [$title]($url)`n"
        }
        else {
            $releaseNotes += "- $title`n"
        }
    }


    if (Test-Path("index.md")) {
        $releaseNotes = @($releaseNotes) + (Get-Content index.md | Select-Object -Skip 1)
    }


    $releaseNotes | Set-Content index.md
}


### Log Previous Commits ###


# Find the last saved commit if it exists.
if (Test-Path .\release.data) {
    $lastCommit = (Get-Content .\release.data).Substring(0, 40)
    $commits = git log -200 --pretty='%H'
    $foundIndex = [array]::indexof($commits, $lastCommit)
    Write-Host "Release Data Found:  $lastCommit at index $foundIndex"
    if ($foundIndex -gt 0) {
        ($foundIndex-1)..0 | ForEach-Object { Save-CommitNotes $_ }
    } else {
        Write-Host "There are no previous commits to log." -ForegroundColor Yellow
    }
} else {
    # Default the search to 20 previous commits.
    20..0 | ForEach-Object { Save-CommitNotes $_ }
}



# Save the Previous Commit
git log -1 --pretty='%H' | Set-Content .\release.data

```