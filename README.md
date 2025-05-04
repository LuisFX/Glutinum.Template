# Glutinum.Template

[![](https://img.shields.io/badge/Sponsors-EA4AAA?style=for-the-badge)](https://mangelmaxime.github.io/sponsors/)

Glutinum.Template is an opinionated template for creating Fable bindings/libraries. It provides a standardized project structure and tooling to help you create high-quality Fable bindings with minimal setup.

> [!NOTE]
> If you are looking for a template to create a standard F# project, you should look at [MiniScaffold](https://github.com/TheAngryByrd/MiniScaffold).

## Features

- Project configuration with validation for common mistakes
    - For example, it will check that you have `FablePackageType` set so the package will be listed in the [Fable package registry](https://fable.io/packages/).
- Enforce commit message conventions via [EasyBuild.CommitLinter](https://github.com/easybuild-org/EasyBuild.CommitLinter)
- Automatic versioning and changelog generation based on the git history
- Enforce code style with [Fantomas](https://fsprojects.github.io/fantomas/)
    - The code is automatically formatted on commit
- Easy release thanks to a `build` orchestrator

## Installation

Install the template with:

```bash
dotnet new install "Glutinum.Template::*"
```

Create a new project with:

```bash
dotnet new glutinum -n MyProject
```

## Documentation

For more detailed information about using the template and the generated project, refer to the [MANUAL.md](content/MANUAL.md) file that will be included in your generated project.

## Getting Started

After creating your project, you'll have the following structure:

```
MyProject/
├── src/                # Your binding source code
├── demo/               # Demo web application to test your binding
├── build/              # Build scripts for CI/CD
├── Directory.*.props   # MSBuild configuration files
└── build.cmd/sh        # Build script entry points
```

### Next steps:

1. **Implement your binding** in the `src/` directory
   ```fsharp
   // src/MyProject.fs
   module MyProject

   open Fable.Core
   open Fable.Core.JsInterop
   
   // Your binding code here
   ```

2. **Create a demo** in the `demo/` directory to showcase your binding

3. **Run the demo locally**
   ```bash
   # Start the demo in watch mode
   ./build.sh demo --watch   # On Unix
   build.cmd demo --watch    # On Windows
   ```

4. **Release your package**
   ```bash
   # Create a release
   ./build.sh release
   ```

## What's Included

The template includes:

- **Source project** - An F# project configured for Fable compilation
- **Demo project** - A simple web application using Vite to test your binding
- **Build system** - Uses [FAKE](https://fake.build/) through EasyBuild for tasks like:
  - Running the demo
  - Publishing to GitHub Pages
  - Creating releases
  - Managing versions
- **Git hooks** - For commit message validation and code formatting

## Troubleshooting

### Common Issues

If you encounter problems with the post-install process, you can still get your template up and running

- **Git Init erros**
  This template expects you to have git installed, otherwise the post install will fail.

- **Build script permissions**: On Unix systems, you may need to make the build script executable:
  ```bash
  chmod +x build.sh
  ```

- **Missing tools**: If you see errors about missing tools, run:
  ```bash
  dotnet tool restore
  ```

## Contributing

If you're interested in contributing to this project, please see the [CONTRIBUTING.md](CONTRIBUTING.md) guide for guidelines, common issues, and how to test your changes locally.