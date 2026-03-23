# Grimmory

![Grimmory Windows CE client](/images/app.png)

Grimmory is a Windows CE handheld client for ISBN-based book workflows against a Grimmory backend.
It targets barcode-scanner usage on legacy mobile devices.

## Tech Stack

- C#
- .NET Compact Framework 3.5
- Windows Forms (Smart Device / Windows CE)
- Visual Studio 2008 solution and project format

## Features

- Username/password login against Grimmory API (`/api/v1/...`)
- Optional auto-connect on app startup
- ISBN search triggered from scanner key `F2`
- Book lookup using ISBN-13 or ISBN-10 (normalized by removing spaces/hyphens)
- If found: displays title, authors, read status, tags, and cover image
- If missing: optional create flow for physical book
- Optional ISBN metadata lookup before creating missing books
- Read status updates pushed to server
- Tag updates pushed to server with debounce (to avoid request spam)

## Solution Structure

- `Grimmory.sln` - Visual Studio 2008 solution
- `Grimmory/Grimmory.csproj` - Windows CE smart-device project (`TargetFrameworkVersion=v3.5`)
- `Grimmory/Program.cs` - application entry point
- `Grimmory/Forms/MainForm*.cs` - UI split by concern:
  - `Forms/MainForm.Authentication.cs` - login and auto-connect
  - `Forms/MainForm.BookSearch.cs` - ISBN search and create flow
  - `Forms/MainForm.BookCover.cs` - asynchronous cover loading
  - `Forms/MainForm.ReadStatus.cs` - read-status update flow
  - `Forms/MainForm.Tags.cs` - tag rendering and debounced updates
  - `Forms/MainForm.Settings.cs` - settings load/save
  - `Forms/MainForm.UIHelpers.cs` - UI state and invoke helpers
- `Grimmory/Forms/LoadingForm*.cs` - loading screen form and designer files
- `Grimmory/Services/ApiService.cs` - HTTP integration layer
- `Grimmory/Services/ApiClientAdapter.cs` - API adapter implementation
- `Grimmory/Abstractions/*.cs` - interfaces for service/settings abstractions
- `Grimmory/Settings/AppSettings.cs` + `Grimmory/Settings/DeviceCrypto.cs` - local settings and credential encryption
- `Grimmory/Settings/FileSettingsStore.cs` - settings persistence implementation
- `Grimmory/Models/*.cs` - request/response/result models and helpers
- `Grimmory/ILMerge/merge_all.bat` - post-build merge script (invokes ILMerge)
- `Grimmory/ILMerge/ILMerge.exe` - bundled ILMerge tool used by the script

## Requirements

To build and deploy as intended:

- Windows machine
- Visual Studio 2008 with Smart Device development support
- .NET Compact Framework 3.5 tooling
- Windows CE / Windows Mobile device or compatible emulator
- Device deployment tooling (for example ActiveSync / Windows Mobile Device Center)

## Build and Deploy

1. Open `Grimmory.sln` in Visual Studio 2008.
2. Select `Debug|AnyCPU` or `Release|AnyCPU`.
3. Build the solution.
4. Deploy from Visual Studio to your configured device/emulator.

Build output:

- `Grimmory/bin/Debug/`
- `Grimmory/bin/Release/`

Post-build behavior:

- `Grimmory.csproj` runs `Grimmory/ILMerge/merge_all.bat` after build.
- The script merges the main executable with `bin/<Configuration>/*.dll`.
- For `Debug`, merged output is written to `bin/Debug/Output/`.
- For `Release`, merged output replaces the original executable in `bin/Release/Output/` and `.dll/.pdb/.xml` files in that folder are removed by the script.

## Runtime Setup

In the app **Settings** tab, configure:

- `Server` (host or `host:port`, without `http://`)
- `User`
- `Password`
- `ISBN Lookup` (optional)
- `Autoconnect` (optional)

Then press **Save**, and use **Connect**.

## Settings Storage

Settings are stored in `settings.txt` next to the executable.

Line order:

1. `server`
2. `user`
3. `password` (stored as `enc:<base64>` in current versions)
4. `isbn lookup` (`true/false` or `1/0`)
5. `autoconnect` (`true/false` or `1/0`)

Notes:

- Legacy plain-text password entries are still readable.
- If encrypted credentials cannot be decrypted on the current device, auto-connect is disabled and the app asks for password re-entry.

## API Contract

`ApiService` uses these endpoints:

- `POST /api/v1/auth/login`
- `GET /api/v1/books`
- `GET /api/v1/books/{id}`
- `POST /api/v1/books/physical`
- `POST /api/v1/books/status`
- `POST /api/v1/books/metadata/isbn-lookup`
- `PUT /api/v1/books/{id}/metadata?mergeCategories=false&replaceMode=REPLACE_WHEN_PROVIDED`
- `GET /api/v1/media/book/{id}/cover?token={accessToken}`

Auth model:

- Login returns bearer token
- Most book endpoints use `Authorization: Bearer <token>`
- Cover endpoint uses `token` query parameter

## Typical Workflow

1. Save settings and connect.
2. Go to **Main** tab.
3. Scan ISBN (or type one) and trigger search with scanner `F2`.
4. If a book exists, edit read status and tags as needed.
5. If a book is missing, confirm add flow (with optional metadata lookup).
6. Cover loads asynchronously when available.

## Operational Notes

- Network requests are plain HTTP (`http://{server}`), not HTTPS.
- API timeout is 10 seconds per request.
- New physical books are created with hardcoded `libraryId = 1`.
- `Newtonsoft.Json.Compact.dll` is vendored in the repository.

## Troubleshooting

- **Login fails / Not authenticated**: verify server, credentials, and reconnect.
- **Search returns not found often**: verify scanner input and backend ISBN data quality.
- **No cover displayed**: cover endpoint may have no image; app falls back to "No cover".
- **Save errors**: ensure the app can write beside the executable.
- **Cannot open/build in modern IDEs**: project is designed for VS2008 + .NET CF.

## License

GNU General Public License v3.0. See `LICENSE`.
