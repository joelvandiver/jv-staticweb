# Haskell (Learning)

## Code Notes

| Code                        | Description                       |
| --------------------------- | --------------------------------- |
| `import`                    | Import a library                  |
| `:module -System.Directory` | Remove System.Directory from ghci |

## Notes

Writing a multiline function in ghci can be accomplished in one of two ways:

1. Use a semicolon at the end of the line:

```haskell
sum [] = 0; sum (n:ns) = n + sum ns
```

2. Use braces:

```haskell
:{
| sum [] = 0
| sum (n:ns) = n + sum ns
:}
```