git commit --amend // Edit the commit message

//Spara ändringar i stash
git stash
git stash pop

//Slå ihop två commits
git reset --soft HEAD~2 // Reset the last 2 commits and keep the changes
git commit -m "New commit message" // Commit the changes to merge the 2 commits
