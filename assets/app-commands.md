# Commands

## Push commits one by one automatically

- Windows (PowerShell):

```powershell
foreach ($rev in $(git rev-list --reverse origin/branch-name..branch-name))
{
    git push origin ($rev + ":branch-name") -f
}
```

- Linux (Shell):

```shell
for rev in $(git rev-list --reverse origin/branch-name..branch-name); do
    git push origin $rev:branch-name -f;
done
```
