# DemandLink Cloud & Local Storage Library

This project is meant for providing a common interface for storing files in the cloud or locally without the user knowing a difference.

## Location of Results

You can find the packed/published Nuget [here](https://dev.azure.com/demandlink/Nuget/_packaging?_a=feed&feed=Nugets)

## Setup

Before you start, you'll need to do some setup work.

### Prerequisites

- Start Docker before running the test project

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

1. **Nuget Authentication**: You'll need to set up credentials for the DemandLink Nuget feed if you haven't already done this for previous demandlink library projects.
   1. You need to generate a Personal Access Token (PAT) from Azure DevOps under your own credentials. Follow steps below:
      1. Got to [DemandLink Azure DevOps](https://dev.azure.com/demandlink) and log in using your demandlink email. If you do not have access, ask an Azure DevOps admin to give you the proper clearance.
      2. Once you're logged in, find the "User Settings" icon in the top right corner next to the circle with your initials.
      3. Select **User Settings** **->** **Personal access tokens**. This will bring up the "Personal Access Tokens" page.
      4. Click on the "New Token" button in the top left corner. From there you'll need the following options:
         1. Token name **whatever you want but make sure you can identify it**.
         2. Organization should be set to **Demandlink**
         3. Expiration **whatever you want (however often you want to change it)**.
         4. Scopes Find the **Packaging** section and select the **Read** checkbox only.
         5. Click **Save**
      5. You will then see a **Success** screen where you can copy the **PAT** string. **NOTE:** make sure to copy before closing, otherwise you'll have to regenerate the token. Once you close that side-nav it will be gone forever.
   2.  Once you have your **PAT** then you can set the environment variables.
      - The username variable name is: `DEMANDLINK_NUGET_USERNAME`
      - The PAT variable name is: `DEMANDLINK_NUGET_PAT`

## Include in Projects

You can include this in projects if your project doesn't yet have the `DemandLink Nuget` feed by either:

1. Adding a `nuget.config` file with the following contents
  ```XML
  <?xml version="1.0" encoding="utf-8"?>
  <configuration>
    <packageSources>
      <clear />
      <!--Other Nuget sources as this will overwrite your current feeds-->
      <!--You can copy the sources from the Nuget installation page in Visual Studio by clicking on the gear icon-->
      <add key="Nugets" value="https://pkgs.dev.azure.com/demandlink/Nuget/_packaging/Nugets/nuget/v3/index.json" />
    </packageSources>
    <packageSourceCredentials>
      <DemandLink_x0020_Nugets>
        <add key="Username" value="%DEMANDLINK_NUGET_USERNAME%" />
        <add key="ClearTextPassword" value="%DEMANDLINK_NUGET_PAT%" />
      </DemandLink_x0020_Nugets>
    </packageSourceCredentials>
  </configuration>
  ```
  to the root of your project and running `dotnet restore` in a CMD or PowerShell window in the root of your project.
2. You can add the above key/value to the Nuget sources in the Nuget installation page in Visual Studio by clicking on the gear icon.

Instructions on how to do the above can be found [here](https://dev.azure.com/demandlink/Nuget/_packaging?_a=connect&feed=Nugets)

## Development

When developing in this project, please follow the following rules.

- Test your work by writing tests for your given scenario and making sure that those tests are passing.
- Please comment any new features since this is a library that others may use.
- All new work and bug fixes must have tests written for it.
- Please follow the outlined namespace structure and code architecture when adding new features. If you are unsure about where a new feature should go then feel free to consult your fellow developers!!
- When/If you add any third-party Nugets to this project, please make sure they are not using anything other than
  `.NetStandard`. If you REALLY need the third-party Nuget then please consult your fellow developers to see whether
  it's really needed. If you *do* end up adding it then please remember to add the `dependency` in the `.nuspec`
  file. You can find instruction on how to do so [here](https://docs.microsoft.com/en-us/nuget/reference/nuspec).

## Deployment

Currently, this project is set up with a CI/CD pipeline based off of the `master` branch. Please follow these steps to publish new work:

- Create a release branch off of the `development` branch.
- Merge the new work to be released into the newly created release branch i.e. `Release-v1.2.3`. Increment the version number in the `project/<project-name>.nuspec` file. So, for example, the version element in the `.nuspec` file should look something like this 
  ```XML
  <?xml version="1.0" encoding="utf-8"?>
  <package >
    <metadata>    
      
      <!--Other configurations-->

      <version>1.2.3</version>
      
      <!--Other configurations-->

    <files>
      <!--your files-->
    </files>
  </package>
  ```
  Or you can always increment the version number before you merge into the release branch (up to you).
- (The below steps are basically from the source control process doc)
- Push the release branch to `origin`
- Merge the release branch into `master`, create a tag such as `v1.2.3`, and push both to `origin`
- That's it. The code will be published to the Azure DevOps Artifact page once it goes through the CI/CD pipeline.

### Manual Deployment

- If the pipeline is broken or unavailable and you need to push manually then you can do so by following these steps
  1. After you have done all the merge work into master above.
  2. Right click on the project name and click `Pack`.
  3. The `dotnet` way
     1. cd into `project/<project-name>/bin/Release`
     2. Run `dotnet nuget push --source "Nugets" --api-key az <package-name.nupkg>`
  4. The `nuget.exe` way
     1. If you don't have the `nuget.exe` downloaded then you'll need to
        1. Download the .Net SDK from [here](https://dotnet.microsoft.com/download) if you don't have it.
        2. Install the `nuget.exe` and **Azure Artifacts Credential Provider** by following the instructions [here](https://github.com/microsoft/artifacts-credprovider#azure-artifacts-credential-provider).
        3. If the above process is out of date, then you'll just have to google how to publish a Nuget file to **Azure Artifacts** from a CMD.
     2. Go into the file location `project/<project-name>/bin/Release/*.[version].nupkg` and copy it.
     3. Paste it into wherever the `nuget.exe` file was copied to (I had it in a folder that also contained the Credential Provider).
     4. Open an CMD, cd into the folder where the `nuget.exe` is (if you didn't add it to your environment variables path) and run the command `nuget.exe push -Source "Nugets" -ApiKey az <your .nupkg file name here>`