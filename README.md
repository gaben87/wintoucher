# WinToucher

WinToucher is a C# WPF application designed to simulate multi-touch input on Windows systems. It leverages the Windows API through P/Invokes to inject touch events. With an intuitive overlay mode, users can draw touch paths with two pointers and execute them with customizable timing.

## Features
- **Overlay Mode**: Draw touch paths on a semi-transparent overlay across the entire screen.
- **Multi-Touch Support**: Supports up to two touch pointers with distinct colors (Red for Pointer 1, Blue for Pointer 2).
- **Customizable Duration**: Set the total duration for touch path execution.
- **Windows API Integration**: Uses P/Invokes to interact with native touch injection APIs.

## Prerequisites
- Windows 10 or later (touch injection requires Windows 8+)
- .NET 8.0+
- Visual Studio 2019 or later

## Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/WinToucher.git
   dotnet build
   ```
## Acknowledgments

    Built with inspiration from [zyf722/wintoucher](https://github.com/zyf722/wintoucher)