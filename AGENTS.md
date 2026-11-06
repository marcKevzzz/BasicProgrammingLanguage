# Project context

This is an ASP.NET Core MVC application targeting .NET 10. Preserve the existing `DELMUNDOMARCKEVIN_241639_` naming convention for model files and types. Read MAINTENANCE.md for architecture and verification commands.

## MonsterASP deployment

The current hosting target is:

- Website: http://programmingfundamentals.runasp.net/
- Web Deploy server: site103843.siteasp.net
- IIS application/site: site103843
- Method: MSDeploy / WMSVC over HTTPS
- Local settings file: `DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING/Properties/PublishProfiles/programmingfundamentals.runasp.net-WebDeploy.publishSettings`

The settings file contains credentials and is deliberately ignored by Git. Read its XML attributes when an authorized deployment needs them; do not print passwords, embed credentials in tracked files, or include them in tool output. The file is local to this checkout; on another computer it must be supplied separately.

This target supersedes the older site103824 profile and basicprogrammingfundamentals.runasp.net hostname. Do not use the obsolete target for new deployments.

Deployment notes are context, not a request to deploy automatically. When deployment is requested, build Release, run the relevant checks, publish to the current profile's target, and verify the live website. Preserve unrelated server files. Web Deploy is installed at `C:\Program Files\IIS\Microsoft Web Deploy V3\msdeploy.exe`.
