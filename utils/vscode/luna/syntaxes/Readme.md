The file `LunaScript.tmLanguage.json` is derived from [TypeScriptReact.tmLanguage](https://github.com/Microsoft/TypeScript-TmLanguage/blob/master/TypeScriptReact.tmLanguage).

To update to the latest version:
- `cd extensions/typescript` and run `npm run update-grammars`
- don't forget to run the integration tests at `./scripts/test-integration.sh`

The script does the following changes:
- fileTypes .tsx -> .luna
- scopeName scope.tsx -> scope.luna
- update all rule names .tsx -> .luna
