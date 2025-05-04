# Project Manual

This manual provides detailed information about using and maintaining your Fable binding project. For a quick overview, refer to the [README.md](./README.md) file.

## Table of Contents

- [Project Structure](#project-structure)
- [Configuration Files](#configuration-files)
- [Development Workflow](#development-workflow)
- [Building and Testing](#building-and-testing)
- [Deployment and Publishing](#deployment-and-publishing)
- [Troubleshooting](#troubleshooting)

## Project Structure

```
YourProject/
├── src/                # Source code of your binding
│   ├── YourProject.fs  # Main binding code
│   └── YourProject.fsproj  # Project file for the binding
├── demo/               # Demo application
│   ├── Main.fs         # Demo application entry point
│   ├── index.html      # Demo HTML template
│   └── vite.config.ts  # Vite configuration
├── build/              # Build scripts
│   ├── Commands/       # Build commands implementation
│   └── EasyBuild.fsproj  # Build project
├── Directory.*.props   # MSBuild property files
├── build.cmd           # Windows build script
└── build.sh            # Unix build script
```

### Key Directories

- **src/**: Contains all the source code for your binding. This is where you'll spend most of your development time.
  
- **demo/**: Contains a demo application that showcases your binding in action. The demo can be:
  - Run locally during development
  - Published to GitHub Pages as an interactive demonstration
  
- **build/**: Contains the build process for this repository, implemented using FAKE and EasyBuild.

## Configuration Files

This project uses MSBuild's Directory.Build.props system for configuration:

### Directory.UserConfig.props

This is where you place configuration unique to your project:

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

### Directory.Packages.props

Contains the list of NuGet packages used in your project. Use this to manage package versions centrally:

```xml
<Project>
    <ItemGroup>
        <PackageVersion Include="Fable.Core" Version="4.0.0" />
        <!-- Add other packages and their versions here -->
    </ItemGroup>
</Project>
```

Adding a package is easy with the `dotnet add package` command:

```bash
# For the main project
dotnet add src package Fable.Core

# For the demo project
dotnet add demo package Fable.Core
```

### Directory.Build.props

Contains default build rules that should work for any project. **Only modify this file if you know what you're doing.**

## Development Workflow

### Setting Up Your Environment

1. Ensure you have the .NET SDK installed
2. Restore tools: `dotnet tool restore`
3. Install JavaScript dependencies:
   ```bash
   cd demo
   npm install  # or pnpm install / yarn
   ```

### Creating Your Binding

1. Edit the files in the `src/` directory
2. Follow Fable binding conventions:
   ```fsharp
   // src/YourProject.fs
   module YourProject
   
   open Fable.Core
   open Fable.Core.JsInterop
   
   // Import JavaScript module/function
   [<Import("something", from="some-js-library")>]
   let something: unit -> unit = jsNative
   
   // Define a type that wraps a JS class
   type MyJsClass =
       [<Emit("new MyJsClass($0)")>]
       abstract Create: options: obj -> MyJsClass
       
       [<Emit("$0.doSomething($1)")>]
       abstract DoSomething: parameter: string -> unit
   ```

3. Update the demo to showcase your binding

### Commit Conventions

This repository uses conventional commit messages to generate releases and changelogs automatically. The commit format is enforced using [EasyBuild.CommitLinter](https://github.com/easybuild-org/EasyBuild.CommitLinter).

Example commit messages:
- `feat: add new binding for X feature`
- `fix: correct parameter type in Y method`
- `docs: improve API documentation`
- `chore: update dependencies`

Note: a commit that doesn't follow the above guidelines will throw an error

## Building and Testing

### Running the Demo

The demo application helps you test your binding in a real-world scenario:

```bash
# On Unix
./build.sh demo --watch

# On Windows
build.cmd demo --watch
```

This will:
1. Build your binding project
2. Start a development server with hot reloading
3. Automatically open a browser with your demo application

The development server is configured to:
- Provide hot module replacement (changes appear instantly)
- Automatically open the browser when started
- Ignore F# source files in watch mode (to prevent double compilation)

### Build Commands

The build script supports various commands:

```bash
# Get help
./build.sh --help    # Unix
build.cmd --help     # Windows

# List all available commands
./build.sh list      # Unix
build.cmd list       # Windows

# Run tests (if you've added them)
./build.sh test      # Unix
build.cmd test       # Windows
```

## Deployment and Publishing

### Publishing to NuGet

Before publishing, you need a NuGet API key saved in the `NUGET_KEY` environment variable. Get one from https://www.nuget.org/account/apikeys.

```bash
# Create a release with default settings
./build.sh release      # Unix
build.cmd release       # Windows

# Create a specific version release
./build.sh release --version 1.2.3    # Unix
build.cmd release --version 1.2.3     # Windows

# Show all release options
./build.sh release --help    # Unix
build.cmd release --help     # Windows
```

The release process:
1. Validates your project
2. Builds the package
3. Publishes to NuGet
4. Creates a GitHub release (if configured)
5. Deploys the demo to GitHub Pages (if enabled in Directory.UserConfig.props)

### GitHub Pages Deployment

GitHub Pages allows you to host a static website directly from your GitHub repository. This is ideal for showcasing your binding with an interactive demo.

#### What is GitHub Pages?

GitHub Pages is a free hosting service that takes HTML, CSS, and JavaScript files straight from a repository on GitHub, optionally runs the files through a build process, and publishes a website.

#### Setting Up GitHub Pages for Your Repository

1. **Enable GitHub Pages in your repository**:
   - Go to your repository on GitHub
   - Navigate to Settings > Pages
   - Set the Source to "GitHub Actions" (recommended) or "Deploy from a branch"
   
2. **Configure deployment in your project**:
   - In `Directory.UserConfig.props`, ensure `<GitHubDeployment>true</GitHubDeployment>` is set
   - Update the `<PackageProjectUrl>` to match your GitHub repository URL

#### Deploying Your Demo

The demo can be automatically published to GitHub Pages:

```bash
./build.sh gh-pages    # Unix
build.cmd gh-pages     # Windows
```

This command:
1. Builds your binding project
2. Builds the demo application with production settings
3. Deploys the built files to the `gh-pages` branch or via GitHub Actions
4. Makes your demo available at `https://your-username.github.io/your-repository`

#### Tips for Effective GitHub Pages Demos

1. **Update the demo's index.html title and metadata** to properly describe your binding
   ```html
   <!-- demo/index.html -->
   <title>My Awesome Binding - Interactive Demo</title>
   <meta name="description" content="Interactive demo for MyBinding - a Fable binding for XYZ library">
   ```

2. **Add usage examples** to make your demo showcase real-world use cases

3. **Include documentation links** in your demo to help users understand how to use your binding

4. **Test your demo thoroughly** before deploying:
   ```bash
   # Test production build locally before deploying
   cd demo
   npm run build
   npm run preview
   ```

5. **Customize the base URL** if needed in `vite.config.ts`:
   ```typescript
   export default defineConfig({
     base: '/your-repository-name/',
     // other config...
   })
   ```

#### Troubleshooting GitHub Pages

- **Deployment fails**: Check your GitHub repository settings and ensure GitHub Pages is properly configured
- **CSS/JS not loading**: Verify the `base` path in vite.config.ts matches your repository name
- **404 errors**: Make sure all asset paths are relative or respect the base URL
- **Custom domain**: If using a custom domain, add it in your GitHub repository settings and create a CNAME file in the demo directory

## Troubleshooting

### Common Issues

1. **Build script not executable** (Unix)
   ```bash
   chmod +x build.sh
   ```

2. **Missing tools**
   ```bash
   dotnet tool restore
   ```

3. **JavaScript dependencies issues**
   ```bash
   cd demo
   rm -rf node_modules
   npm install
   ```

4. **Release validation fails**
   - Check that all required properties are set in Directory.UserConfig.props
   - Ensure your git workspace is clean
   - Verify that your commit messages follow the convention

### Getting Help

If you encounter issues not covered here, please check:
- The [Fable documentation](https://fable.io)
- The [EasyBuild documentation](https://github.com/easybuild-org/EasyBuild)
- Open an issue in your project repository
