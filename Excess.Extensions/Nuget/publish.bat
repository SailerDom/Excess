
..\..\.nuget\nuget pack Excess.NInjector.nuspec
..\..\.nuget\nuget push Excess.Extensions.NInjector.0.48.10-alpha.nupkg

del Excess.Extensions.NInjector.0.48.10-alpha.nupkg

..\..\.nuget\nuget pack Excess.Dapper.nuspec
..\..\.nuget\nuget push Excess.Extensions.Dapper.0.48.10-alpha.nupkg

del Excess.Extensions.Dapper.0.48.10-alpha.nupkg

..\..\.nuget\nuget pack Excess.Extensions.nuspec
..\..\.nuget\nuget push Excess.Extensions.0.48.10-alpha.nupkg

del Excess.Extensions.0.48.10-alpha.nupkg