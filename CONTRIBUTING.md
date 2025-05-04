# Contributing to Glutinum.Template

Thank you for your interest in contributing to Glutinum.Template! This document outlines the process for contributing to this project and provides guidance on common issues you might encounter.

## Project Structure

The project has the following structure:

```
Glutinum.Template/
├── build/                  # Build infrastructure for this repository
│   ├── Commands/           # Build commands implementation
│   ├── Utils/              # Build utilities
│   ├── EasyBuild.fsproj    # Build project
│   ├── Main.fs             # Command-line interface entry point
│   └── Workspace.fs        # File system abstractions
├── content/                # Template content that gets generated
│   ├── build/              # Build scripts for the generated project
│   ├── demo/               # Demo application for testing
│   │   ├── index.html      # Demo HTML template
│   │   ├── Main.fs         # Demo entry point
│   │   └── vite.config.ts  # Vite configuration
│   ├── src/                # Source code for the binding
│   │   ├── GlueTemplate.fs # Main binding code (renamed on generation)
│   │   └── GlueTemplate.fsproj # Project file for the binding
│   ├── .template.config/   # Template configuration
│   │   └── template.json   # Template settings and post-actions
│   ├── build.cmd           # Windows build script
│   ├── build.sh            # Unix build script
│   ├── Directory.*.props   # MSBuild property files
│   └── ...                 # Other configuration files
├── Glutinum.Template.proj  # Template packaging definition
├── build.cmd               # Build script (Windows)
├── build.sh                # Build script (Unix)
├── README.md               # Main documentation
└── ...                     # Other files
```

## MSBuild Property Files

The template uses three important MSBuild property files to control build behavior and project configuration:

### Directory.UserConfig.props

This file is meant to be customized by the end user of the template. It contains project-specific settings:

```xml
<Project>
    <PropertyGroup>
        <!-- Package metadata -->
        <Authors>Your Name</Authors>
        <PackageProjectUrl>https://github.com/your-username/your-project</PackageProjectUrl>
        <RepositoryUrl>https://github.com/your-username/your-project.git</RepositoryUrl>
        
        <!-- Fable specific -->
        <FablePackageType>binding</FablePackageType>
        <AutoGenerateBindingRedirects>true</AutoGenerateBindingRedirects>
        
        <!-- Demo configuration -->
        <GitHubDeployment>true</GitHubDeployment>
    </PropertyGroup>
</Project>
```

**Key Properties:**
- `Authors`: Package author name(s)
- `PackageProjectUrl`: URL to the project website/repository
- `RepositoryUrl`: Git repository URL
- `FablePackageType`: Sets the Fable package type (important for registry listing)
- `GitHubDeployment`: Enables GitHub Pages deployment for the demo

### Directory.Packages.props

This file implements central package version management, allowing all package versions to be specified in one place:

```xml
<Project>
    <ItemGroup>
        <PackageVersion Include="Fable.Core" Version="4.0.0" />
        <!-- Add other packages and their versions here -->
    </ItemGroup>
</Project>
```

**Key Concepts:**
- Package versions are defined once in this file
- Projects reference packages without specifying versions
- Helps maintain consistent dependencies across projects
- Updates to package versions only need to be made in one place

### Directory.Build.props

This file provides default MSBuild properties and behaviors that apply to all projects. Users should generally not modify this file unless they understand MSBuild well.

It handles:
- Default MSBuild settings
- Fable configuration
- Project conventions
- Shared build properties

## Build System Architecture

The build system in the `build/` directory is responsible for packaging and releasing the template. Understanding this system is important for contributors who want to add or modify build functionality.

### Dependencies

The build system uses several dependencies, each with a specific purpose:

| Package | Purpose |
|---------|---------|
| `BlackFox.CommandLine` | Utilities for building command-line arguments |
| `EasyBuild.FileSystemProvider` | Type-safe file system access |
| `EasyBuild.Tools` | Common build tools (changelog generation, git operations) |
| `Semver` | Semantic versioning manipulation |
| `SimpleExec` | Process execution helpers |
| `Spectre.Console.Cli` | Command-line interface framework |

### Key Components

1. **Main.fs** - Entry point that configures the command-line application
   - Registers available commands
   - Sets up command descriptions
   - Initializes git hooks

2. **Workspace.fs** - Provides strongly-typed file paths
   - Uses `EasyBuild.FileSystemProvider` for type-safe file access
   - Defines the structure of the workspace

3. **Commands/** - Contains command implementations
   - Each command is a class implementing `Command<T>`
   - Commands handle specific build actions (e.g., Release)

4. **Utils/** - Helper modules and functions
   - Changelog parsing and manipulation
   - .NET command execution
   - Other utility functions

### Adding a New Build Command

To add a new build command:

1. **Create a command settings class**:
   ```fsharp
   type MyCommandSettings() =
       inherit CommandSettings()
       // Add command-specific settings properties
   ```

2. **Create a command class**:
   ```fsharp
   type MyCommand() =
       inherit Command<MyCommandSettings>()
       
       override __.Execute(context, settings) =
           // Command implementation
           0  // Return 0 for success
   ```

3. **Register the command in Main.fs**:
   ```fsharp
   config
       .AddCommand<MyCommand>("my-command")
       .WithDescription("Description of the command")
       |> ignore
   ```

4. **Implement command functionality** using the utilities in the Utils folder and other EasyBuild tools

## Local Development and Testing

### Making Changes

1. Edit the files in the content directory as needed.
2. Ensure that JSON files (like `content/.template.config/template.json`) are valid. Remember that JSON does not allow comments.

### Testing Your Changes

After making changes, you can test them locally without publishing:

```bash
# From the project root directory
dotnet new install . --force
```

This will install your local version of the template. Then create a new project:

```bash
# Change to a different directory
cd /path/to/test/directory

# Create a new project with your template
dotnet new glutinum -n TestProject
```

### Cross-Platform Testing

Since Glutinum.Template is designed to work across different operating systems, it's **essential** to test your changes on both Windows and Unix-based systems (macOS, Linux) before submitting a pull request.

#### Testing Recommendations

1. **Test on all platforms you have access to**
   - Test the template installation: `dotnet new install .`
   - Test project creation: `dotnet new glutinum -n TestProject`
   - Test post-actions: Verify git init and hooks setup work correctly
   - Test the build commands: `build.cmd`/`build.sh` in the generated project

2. **Post-action scripts** are particularly sensitive to platform differences:
   - Windows uses `.cmd` files
   - Unix uses `.sh` files with execute permissions
   - Path separators differ (`\` vs `/`)
   - Line endings matter (`CRLF` vs `LF`)

3. **If you don't have access to all platforms**:
   - Consider using a virtual machine, Docker, or WSL
   - Set up a simple GitHub Actions workflow to test your changes
   - Ask for help from other contributors for platform-specific testing
   - Clearly mention in your PR which platforms you've tested on

4. **Common platform-specific issues to check**:
   - File permissions (especially for shell scripts on Unix)
   - Executable paths and environment assumptions
   - Path separators in template.json post-actions
   - Line ending consistency

#### Testing Matrix

| Test | Windows | macOS/Linux |
|------|---------|-------------|
| Template installation | `dotnet new install .` | `dotnet new install .` |
| Project creation | `dotnet new glutinum -n Test` | `dotnet new glutinum -n Test` |
| Git initialization | Should run automatically | Should run automatically |
| Build script execution | `build.cmd` works | `./build.sh` works (may need `chmod +x`) |
| Demo application | `build.cmd demo --watch` opens browser automatically | `./build.sh demo --watch` opens browser automatically |

## Common Issues and Solutions

### Post-Action Failures

If post-actions fail during template instantiation, check:

1. **Template.json Configuration**: Ensure all post-actions have the correct properties. For example, each action that executes a command should have:
   ```json
   "args": {
       "executable": "command-name",
       "args": "command-arguments",
       "redirectStandardOutput": false,
       "redirectStandardError": false
   }
   ```

2. **File Existence**: Ensure that referenced scripts (like build.cmd or build.sh) exist in the template content.

3. **Script Compatibility**: Ensure scripts are compatible with both Windows and Unix environments.

### JSON Validation

- JSON files in the template (e.g., `template.json`) must be valid JSON:
  - No comments are allowed (remove lines with `//`)
  - Proper quote usage and comma placement

## Making a Pull Request

1. Create a descriptive branch for your changes
2. Make your changes and test them locally
3. Update documentation to reflect your changes
4. Submit a pull request with a clear description of:
   - The problem you're solving
   - How your changes address it
   - Any testing you've done

## Release Process

The maintainers of this project handle the release process. New releases are created based on changes in the main branch and follow semantic versioning. 