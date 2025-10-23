# AMCode Common Components Library

This project contains a number of common components that are frequently used across applications.

## Quick Start

For detailed build instructions and local development setup, see [DEVELOPMENT.md](DEVELOPMENT.md).

## Location of Results

You can find the packed/published Nuget packages in the local packages directory after building.

## Setup

Before you start, you'll need to do some setup work.

### Prerequisites

### Setting an environment variable

You'll need to set environment variables for sensitive information so you can achieve that with the below:

- Set the user name variable in a command window with
  
  ```sh
    setx ENVIRONMENT_VARIABLE_NAME "<environment-variable-value>"
  ```

- or in PowerShell

  ```PowerShell
    [Environment]::SetEnvironmentVariable("ENVIRONMENT_VARIABLE_NAME", "environment-variable-value", "User")
    ```

- To verify, open another CMD window and run the following commands. You should see the values you assigned.
  
  ```sh
    echo %ENVIRONMENT_VARIABLE_NAME%
  ```

- Once you can see them in the `echo`, you're ready to go. If you can't see them in the `echo` try opening the **Environment Variables** window by searching for **Edit the system environment variables** in Windows search bar. You should see the values under the user environment variables.

### Global Solution Variables

1. **Nuget Authentication**: You'll need to set up credentials for the AMCode Nuget feed if you haven't already done this for previous AMCode library projects.
   1. You need to generate a Personal Access Token (PAT) from Azure DevOps under your own credentials. Follow steps below:
      1. Got to Azure DevOps and log in using your email. If you do not have access, ask an Azure DevOps admin to give you the proper clearance.
      2. Once you're logged in, find the "User Settings" icon in the top right corner next to the circle with your initials.
      3. Select **User Settings** **->** **Personal access tokens**. This will bring up the "Personal Access Tokens" page.
      4. Click on the "New Token" button in the top left corner. From there you'll need the following options:
         1. Token name **whatever you want but make sure you can identify it**.
         2. Organization should be set to your organization
         3. Expiration **whatever you want (however often you want to change it)**.
         4. Scopes Find the **Packaging** section and select the **Read** checkbox only.
      5. Click **Create** and copy the token value.
   2. Now you need to add the token to your environment variables:
      1. Open the **Environment Variables** window by searching for **Edit the system environment variables** in Windows search bar.
      2. Click **Environment Variables** button.
      3. Under **User variables** click **New**.
      4. Variable name: `AMCODE_NUGET_USERNAME`
      5. Variable value: Your email address
      6. Click **OK**.
      7. Under **User variables** click **New** again.
      8. Variable name: `AMCODE_NUGET_PAT`
      9. Variable value: The token you copied from step 1.5
      10. Click **OK**.
      11. Click **OK** on the Environment Variables window.
      12. Click **OK** on the System Properties window.
   3. Now you need to restart Visual Studio or your IDE for the environment variables to take effect.

## Development

For detailed development instructions, see [DEVELOPMENT.md](DEVELOPMENT.md).

## Deployment

To deploy this project:

1. Build the solution in Release mode
2. Run the unit tests to ensure everything is working
3. Create a NuGet package using `dotnet pack -c Release`
4. The package will be available in the `bin/Release` directory

## Contributing

1. Make your changes
2. Run the unit tests to ensure nothing is broken
3. Build the solution to ensure it compiles
4. Create a pull request with your changes