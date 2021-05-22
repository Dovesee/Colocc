using System.Reflection;  
using System.Resources;  
using System.Runtime.CompilerServices;  
using System.Runtime.InteropServices;  
using System.Windows;  
[assembly: AssemblyTitle(ColoccAssembly.Title)]  
[assembly: AssemblyDescription("")]  
[assembly: AssemblyConfiguration("")]  
[assembly: AssemblyCompany("")]  
[assembly: AssemblyProduct("Colocc")]  
[assembly: AssemblyCopyright("Copyright ©  2021")]  
[assembly: AssemblyTrademark(ColoccAssembly.BuildTime+"GIT COMMIT"+ColoccAssembly.GitVersionCommit)]  
[assembly: AssemblyCulture("")]  
[assembly: ComVisible(false)]  
[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly )]  
﻿[assembly: AssemblyVersion(ColoccAssembly.Version)]   
[assembly: AssemblyFileVersion(ColoccAssembly.Version)]  
internal static class ColoccAssembly 
{ 
    public const string Title = "Colocc"; 
    public const string Version = "1.0.0." + GitVersionCount; 
    public const string BuildTime = "2021/05/22 15:56"; 
    public const string GitVersionCommit = "faf3324"; 
    private const string GitVersionCount = "7"; 
} 
