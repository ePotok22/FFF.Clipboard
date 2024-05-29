# Clipboard

## Overview
Welcome to Clipboard, a robust C# library designed to extend and enhance clipboard functionality on Windows platforms. This library provides developers with tools to manage clipboard history and improve the efficiency of copy-paste operations.

## Features
- Clipboard History Management: Tracks and manages the history of clipboard contents.
- Easy Integration: Designed to be easily integrated into .NET applications.

## Quick Start
### Prerequisites
Ensure you have .NET Framework 4.6.2 or higher installed.

### Installation
Clone the repository using Git:

``` csharp
Copy code
git clone https://github.com/ePotok22/Clipboard.git
```

Build the project:

``` csharp
dotnet build
```
### Usage
Here’s a simple example of how to use Clipboard to access clipboard history:

``` csharp
using FFF.Clipboard;

// Get the latest item from the clipboard
string text = ClipboardManager.GetText(TextDataFormat.Text);

Console.WriteLine("Last copied text: " + text);
```
## Contributing
We welcome contributions from the community. Please read our CONTRIBUTING.md file for guidelines on how to make contributions.

## License
This project is licensed under the MIT License - see the LICENSE file for details.
