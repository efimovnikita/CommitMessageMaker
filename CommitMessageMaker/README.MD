# Commit Message Maker

The Commit Message Maker is a command-line interface (CLI) tool that leverages the OpenAI API to generate smart commit messages based on the changes in your `git diff`. Built with developer convenience in mind, it produces messages that follow the conventional commit format.

## Features

- Consumes `git diff` output to understand code changes.
- Produces conventionally formatted commit messages using OpenAI's GPT-3.
- Easily configurable through a JSON-based settings file.

## Prerequisites

- .NET 8.0 SDK or later.
- An active OpenAI API key.

## Configuration

Before using the tool, you must provide your OpenAI API key and an optional proxy address in a configuration file named `appsettings.json`, located at `.config/cmm` in your user profile directory.

### Configuration File Structure

```json
{
  "Settings": {
    "OpenAiApiKey": "YOUR_OPENAI_API_KEY",
    "ProxyAddress": "YOUR_OPTIONAL_PROXY_ADDRESS"
  }
}
```

## Installation as a .NET Tool

Commit Message Maker can be packed and installed as a .NET global tool, which allows it to be used from any directory in your system.

### Packing the Tool

To pack Commit Message Maker, run the following command from the root of the source code:

```bash
dotnet pack
```

This will generate a .nupkg file in the `./nupkg` directory.

### Installing the Tool

Once you have the .nupkg file, you can install Commit Message Maker globally using:

```bash
dotnet tool install --global --add-source ./nupkg CommitMessageMaker
```

Upon installation, the tool can be invoked using the command `cmm`.

## Usage

After installation, you can run Commit Message Maker by piping the output of `git diff` directly into it:

```bash
git diff | cmm
```

The generated commit message will be output to your command line.

## Contributing

Contributors are welcome! Please feel free to submit pull requests with any improvements or bug fixes. Remember to follow the existing coding style and explain your changes clearly.

## License

The Commit Message Maker is open-sourced under the MIT License. See the [LICENSE.md](LICENSE.md) file for more information (if not present, you should create this file).

## Issues and Support

For bug reports, feature requests, or any questions, please open an issue in the GitHub repository. We're here to help.

---

**Disclaimer**: Use of this tool complies with OpenAI's usage policy. Users are responsible for any associated costs with the OpenAI API.

**Note**: Always ensure your API key is secure. Review your configuration regularly to prevent any unintended charges.