using System;
using System.IO;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Domain.Interfaces;
using AMCode.Documents.Xlsx.Domain.Models;
using AMCode.Documents.Xlsx.Infrastructure.Adapters;
using AMCode.Documents.Xlsx.Infrastructure.Interfaces;
using AMCode.Xlsx;

namespace AMCode.Documents.Xlsx.Infrastructure.Factories
{
    /// <summary>
    /// Factory implementation for creating and opening workbooks
    /// Provides dependency injection support and comprehensive error handling
    /// </summary>
    public class WorkbookFactory : IWorkbookFactory
    {
        private readonly IWorkbookEngine _engine;
        private readonly IWorkbookLogger _logger;
        private readonly IWorkbookValidator _validator;

        /// <summary>
        /// Initializes a new instance of the WorkbookFactory class
        /// </summary>
        /// <param name="engine">The workbook engine for low-level operations</param>
        /// <param name="logger">The logger for operation tracking</param>
        /// <param name="validator">The validator for workbook validation</param>
        /// <exception cref="ArgumentNullException">Thrown when any dependency is null</exception>
        public WorkbookFactory(IWorkbookEngine engine, IWorkbookLogger logger, IWorkbookValidator validator)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        /// <summary>
        /// Creates a new empty workbook
        /// </summary>
        /// <returns>Result containing the new workbook or error information</returns>
        public Result<IWorkbookDomain> CreateWorkbook()
        {
            try
            {
                _logger.LogWorkbookOperation("Creating new empty workbook", Guid.Empty);
                
                var engineResult = _engine.Create();
                if (!engineResult.IsSuccess)
                {
                    _logger.LogError("Failed to create workbook engine", engineResult.Error, Guid.Empty);
                    return Result<IWorkbookDomain>.Failure(engineResult.Error);
                }

                var workbook = CreateWorkbookFromEngine(engineResult.Value);
                if (workbook.IsSuccess)
                {
                    _logger.LogInformation($"Successfully created workbook with ID: {workbook.Value.Id}", workbook.Value.Id);
                }

                return workbook;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error creating workbook", ex, Guid.Empty);
                return Result<IWorkbookDomain>.Failure($"Unexpected error creating workbook: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Creates a new workbook with the specified title
        /// </summary>
        /// <param name="title">The title for the new workbook</param>
        /// <returns>Result containing the new workbook or error information</returns>
        public Result<IWorkbookDomain> CreateWorkbook(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Result<IWorkbookDomain>.Failure("Title cannot be null or empty");
            }

            try
            {
                _logger.LogWorkbookOperation($"Creating new workbook with title: {title}", Guid.Empty);
                
                var engineResult = _engine.Create(title);
                if (!engineResult.IsSuccess)
                {
                    _logger.LogError("Failed to create workbook engine with title", engineResult.Error, Guid.Empty);
                    return Result<IWorkbookDomain>.Failure(engineResult.Error);
                }

                var workbook = CreateWorkbookFromEngine(engineResult.Value);
                if (workbook.IsSuccess)
                {
                    workbook.Value.Title = title;
                    _logger.LogInformation($"Successfully created workbook with title '{title}' and ID: {workbook.Value.Id}", workbook.Value.Id);
                }

                return workbook;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error creating workbook with title '{title}'", ex, Guid.Empty);
                return Result<IWorkbookDomain>.Failure($"Unexpected error creating workbook: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Creates a new workbook with the specified title and author
        /// </summary>
        /// <param name="title">The title for the new workbook</param>
        /// <param name="author">The author for the new workbook</param>
        /// <returns>Result containing the new workbook or error information</returns>
        public Result<IWorkbookDomain> CreateWorkbook(string title, string author)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Result<IWorkbookDomain>.Failure("Title cannot be null or empty");
            }

            if (string.IsNullOrWhiteSpace(author))
            {
                return Result<IWorkbookDomain>.Failure("Author cannot be null or empty");
            }

            try
            {
                _logger.LogWorkbookOperation($"Creating new workbook with title: {title}, author: {author}", Guid.Empty);
                
                var engineResult = _engine.Create(title);
                if (!engineResult.IsSuccess)
                {
                    _logger.LogError("Failed to create workbook engine with title and author", engineResult.Error, Guid.Empty);
                    return Result<IWorkbookDomain>.Failure(engineResult.Error);
                }

                var workbook = CreateWorkbookFromEngine(engineResult.Value);
                if (workbook.IsSuccess)
                {
                    workbook.Value.Title = title;
                    workbook.Value.Author = author;
                    _logger.LogInformation($"Successfully created workbook with title '{title}', author '{author}' and ID: {workbook.Value.Id}", workbook.Value.Id);
                }

                return workbook;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error creating workbook with title '{title}' and author '{author}'", ex, Guid.Empty);
                return Result<IWorkbookDomain>.Failure($"Unexpected error creating workbook: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Creates a new workbook with the specified metadata
        /// </summary>
        /// <param name="metadata">The metadata for the new workbook</param>
        /// <returns>Result containing the new workbook or error information</returns>
        public Result<IWorkbookDomain> CreateWorkbook(WorkbookCreationMetadata metadata)
        {
            if (string.IsNullOrWhiteSpace(metadata.Title))
            {
                return Result<IWorkbookDomain>.Failure("Title cannot be null or empty");
            }

            try
            {
                _logger.LogWorkbookOperation($"Creating new workbook with metadata: {metadata.Title}", Guid.Empty);
                
                var engineResult = _engine.Create(metadata);
                if (!engineResult.IsSuccess)
                {
                    _logger.LogError("Failed to create workbook engine with metadata", engineResult.Error, Guid.Empty);
                    return Result<IWorkbookDomain>.Failure(engineResult.Error);
                }

                var workbook = CreateWorkbookFromEngine(engineResult.Value);
                if (workbook.IsSuccess)
                {
                    ApplyMetadataToWorkbook(workbook.Value, metadata);
                    _logger.LogInformation($"Successfully created workbook with metadata and ID: {workbook.Value.Id}", workbook.Value.Id);
                }

                return workbook;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error creating workbook with metadata", ex, Guid.Empty);
                return Result<IWorkbookDomain>.Failure($"Unexpected error creating workbook: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Opens an existing workbook from a stream
        /// </summary>
        /// <param name="stream">The stream containing the workbook data</param>
        /// <returns>Result containing the opened workbook or error information</returns>
        public Result<IWorkbookDomain> OpenWorkbook(Stream stream)
        {
            if (stream == null)
            {
                return Result<IWorkbookDomain>.Failure("Stream cannot be null");
            }

            try
            {
                _logger.LogWorkbookOperation("Opening workbook from stream", Guid.Empty);
                
                var engineResult = _engine.Open(stream);
                if (!engineResult.IsSuccess)
                {
                    _logger.LogError("Failed to open workbook from stream", engineResult.Error, Guid.Empty);
                    return Result<IWorkbookDomain>.Failure(engineResult.Error);
                }

                var workbook = CreateWorkbookFromEngine(engineResult.Value);
                if (workbook.IsSuccess)
                {
                    _logger.LogInformation($"Successfully opened workbook from stream with ID: {workbook.Value.Id}", workbook.Value.Id);
                }

                return workbook;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error opening workbook from stream", ex, Guid.Empty);
                return Result<IWorkbookDomain>.Failure($"Unexpected error opening workbook: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Opens an existing workbook from a stream with specified options
        /// </summary>
        /// <param name="stream">The stream containing the workbook data</param>
        /// <param name="options">The options for opening the workbook</param>
        /// <returns>Result containing the opened workbook or error information</returns>
        public Result<IWorkbookDomain> OpenWorkbook(Stream stream, WorkbookOpenOptions options)
        {
            if (stream == null)
            {
                return Result<IWorkbookDomain>.Failure("Stream cannot be null");
            }

            try
            {
                _logger.LogWorkbookOperation("Opening workbook from stream with options", Guid.Empty);
                
                var engineResult = _engine.Open(stream, options);
                if (!engineResult.IsSuccess)
                {
                    _logger.LogError("Failed to open workbook from stream with options", engineResult.Error, Guid.Empty);
                    return Result<IWorkbookDomain>.Failure(engineResult.Error);
                }

                var workbook = CreateWorkbookFromEngine(engineResult.Value);
                if (workbook.IsSuccess)
                {
                    _logger.LogInformation($"Successfully opened workbook from stream with options and ID: {workbook.Value.Id}", workbook.Value.Id);
                }

                return workbook;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error opening workbook from stream with options", ex, Guid.Empty);
                return Result<IWorkbookDomain>.Failure($"Unexpected error opening workbook: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Opens an existing workbook from a file path
        /// </summary>
        /// <param name="filePath">The file path to the workbook</param>
        /// <returns>Result containing the opened workbook or error information</returns>
        public Result<IWorkbookDomain> OpenWorkbook(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return Result<IWorkbookDomain>.Failure("File path cannot be null or empty");
            }

            if (!File.Exists(filePath))
            {
                return Result<IWorkbookDomain>.Failure($"File does not exist: {filePath}");
            }

            try
            {
                _logger.LogWorkbookOperation($"Opening workbook from file: {filePath}", Guid.Empty);
                
                var engineResult = _engine.Open(filePath);
                if (!engineResult.IsSuccess)
                {
                    _logger.LogError($"Failed to open workbook from file: {filePath}", engineResult.Error, Guid.Empty);
                    return Result<IWorkbookDomain>.Failure(engineResult.Error);
                }

                var workbook = CreateWorkbookFromEngine(engineResult.Value);
                if (workbook.IsSuccess)
                {
                    _logger.LogInformation($"Successfully opened workbook from file '{filePath}' with ID: {workbook.Value.Id}", workbook.Value.Id);
                }

                return workbook;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error opening workbook from file: {filePath}", ex, Guid.Empty);
                return Result<IWorkbookDomain>.Failure($"Unexpected error opening workbook: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Opens an existing workbook from a file path with specified options
        /// </summary>
        /// <param name="filePath">The file path to the workbook</param>
        /// <param name="options">The options for opening the workbook</param>
        /// <returns>Result containing the opened workbook or error information</returns>
        public Result<IWorkbookDomain> OpenWorkbook(string filePath, WorkbookOpenOptions options)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return Result<IWorkbookDomain>.Failure("File path cannot be null or empty");
            }

            if (!File.Exists(filePath))
            {
                return Result<IWorkbookDomain>.Failure($"File does not exist: {filePath}");
            }

            try
            {
                _logger.LogWorkbookOperation($"Opening workbook from file with options: {filePath}", Guid.Empty);
                
                var engineResult = _engine.Open(filePath, options);
                if (!engineResult.IsSuccess)
                {
                    _logger.LogError($"Failed to open workbook from file with options: {filePath}", engineResult.Error, Guid.Empty);
                    return Result<IWorkbookDomain>.Failure(engineResult.Error);
                }

                var workbook = CreateWorkbookFromEngine(engineResult.Value);
                if (workbook.IsSuccess)
                {
                    _logger.LogInformation($"Successfully opened workbook from file '{filePath}' with options and ID: {workbook.Value.Id}", workbook.Value.Id);
                }

                return workbook;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error opening workbook from file with options: {filePath}", ex, Guid.Empty);
                return Result<IWorkbookDomain>.Failure($"Unexpected error opening workbook: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Creates a workbook from a template
        /// </summary>
        /// <param name="templatePath">The path to the template file</param>
        /// <returns>Result containing the new workbook or error information</returns>
        public Result<IWorkbookDomain> CreateFromTemplate(string templatePath)
        {
            if (string.IsNullOrWhiteSpace(templatePath))
            {
                return Result<IWorkbookDomain>.Failure("Template path cannot be null or empty");
            }

            if (!File.Exists(templatePath))
            {
                return Result<IWorkbookDomain>.Failure($"Template file does not exist: {templatePath}");
            }

            try
            {
                _logger.LogWorkbookOperation($"Creating workbook from template: {templatePath}", Guid.Empty);
                
                using (var templateStream = File.OpenRead(templatePath))
                {
                    var result = CreateFromTemplate(templateStream);
                    if (result.IsSuccess)
                    {
                        _logger.LogInformation($"Successfully created workbook from template '{templatePath}' with ID: {result.Value.Id}", result.Value.Id);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error creating workbook from template: {templatePath}", ex, Guid.Empty);
                return Result<IWorkbookDomain>.Failure($"Unexpected error creating workbook from template: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Creates a workbook from a template with specified options
        /// </summary>
        /// <param name="templatePath">The path to the template file</param>
        /// <param name="options">The options for creating the workbook</param>
        /// <returns>Result containing the new workbook or error information</returns>
        public Result<IWorkbookDomain> CreateFromTemplate(string templatePath, WorkbookCreationOptions options)
        {
            if (string.IsNullOrWhiteSpace(templatePath))
            {
                return Result<IWorkbookDomain>.Failure("Template path cannot be null or empty");
            }

            if (!File.Exists(templatePath))
            {
                return Result<IWorkbookDomain>.Failure($"Template file does not exist: {templatePath}");
            }

            try
            {
                _logger.LogWorkbookOperation($"Creating workbook from template with options: {templatePath}", Guid.Empty);
                
                using (var templateStream = File.OpenRead(templatePath))
                {
                    var result = CreateFromTemplate(templateStream, options);
                    if (result.IsSuccess)
                    {
                        _logger.LogInformation($"Successfully created workbook from template '{templatePath}' with options and ID: {result.Value.Id}", result.Value.Id);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error creating workbook from template with options: {templatePath}", ex, Guid.Empty);
                return Result<IWorkbookDomain>.Failure($"Unexpected error creating workbook from template: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Creates a workbook from a template stream
        /// </summary>
        /// <param name="templateStream">The stream containing the template data</param>
        /// <returns>Result containing the new workbook or error information</returns>
        public Result<IWorkbookDomain> CreateFromTemplate(Stream templateStream)
        {
            if (templateStream == null)
            {
                return Result<IWorkbookDomain>.Failure("Template stream cannot be null");
            }

            try
            {
                _logger.LogWorkbookOperation("Creating workbook from template stream", Guid.Empty);
                
                // For now, we'll treat template creation as opening the template
                // In a more sophisticated implementation, this would create a copy
                var result = OpenWorkbook(templateStream);
                if (result.IsSuccess)
                {
                    _logger.LogInformation($"Successfully created workbook from template stream with ID: {result.Value.Id}", result.Value.Id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error creating workbook from template stream", ex, Guid.Empty);
                return Result<IWorkbookDomain>.Failure($"Unexpected error creating workbook from template: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Creates a workbook from a template stream with specified options
        /// </summary>
        /// <param name="templateStream">The stream containing the template data</param>
        /// <param name="options">The options for creating the workbook</param>
        /// <returns>Result containing the new workbook or error information</returns>
        public Result<IWorkbookDomain> CreateFromTemplate(Stream templateStream, WorkbookCreationOptions options)
        {
            if (templateStream == null)
            {
                return Result<IWorkbookDomain>.Failure("Template stream cannot be null");
            }

            try
            {
                _logger.LogWorkbookOperation("Creating workbook from template stream with options", Guid.Empty);
                
                // For now, we'll treat template creation as opening the template
                // In a more sophisticated implementation, this would create a copy
                var result = OpenWorkbook(templateStream);
                if (result.IsSuccess)
                {
                    _logger.LogInformation($"Successfully created workbook from template stream with options and ID: {result.Value.Id}", result.Value.Id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error creating workbook from template stream with options", ex, Guid.Empty);
                return Result<IWorkbookDomain>.Failure($"Unexpected error creating workbook from template: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Clones an existing workbook
        /// </summary>
        /// <param name="workbook">The workbook to clone</param>
        /// <returns>Result containing the cloned workbook or error information</returns>
        public Result<IWorkbookDomain> CloneWorkbook(IWorkbookDomain workbook)
        {
            if (workbook == null)
            {
                return Result<IWorkbookDomain>.Failure("Workbook cannot be null");
            }

            try
            {
                _logger.LogWorkbookOperation($"Cloning workbook with ID: {workbook.Id}", workbook.Id);
                
                // For now, we'll create a new workbook and copy properties
                // In a more sophisticated implementation, this would create a deep copy
                var result = CreateWorkbook(workbook.Title, workbook.Author);
                if (result.IsSuccess)
                {
                    // Copy additional properties
                    result.Value.Subject = workbook.Subject;
                    result.Value.Company = workbook.Company;
                    result.Value.Category = workbook.Category;
                    result.Value.Keywords = workbook.Keywords;
                    result.Value.Comments = workbook.Comments;
                    result.Value.Manager = workbook.Manager;
                    result.Value.Application = workbook.Application;
                    result.Value.ApplicationVersion = workbook.ApplicationVersion;
                    result.Value.Template = workbook.Template;
                    
                    _logger.LogInformation($"Successfully cloned workbook with ID: {result.Value.Id}", result.Value.Id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error cloning workbook with ID: {workbook.Id}", ex, workbook.Id);
                return Result<IWorkbookDomain>.Failure($"Unexpected error cloning workbook: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Clones an existing workbook with specified options
        /// </summary>
        /// <param name="workbook">The workbook to clone</param>
        /// <param name="options">The options for cloning the workbook</param>
        /// <returns>Result containing the cloned workbook or error information</returns>
        public Result<IWorkbookDomain> CloneWorkbook(IWorkbookDomain workbook, WorkbookCloneOptions options)
        {
            if (workbook == null)
            {
                return Result<IWorkbookDomain>.Failure("Workbook cannot be null");
            }

            try
            {
                _logger.LogWorkbookOperation($"Cloning workbook with options and ID: {workbook.Id}", workbook.Id);
                
                var result = CloneWorkbook(workbook);
                if (result.IsSuccess)
                {
                    _logger.LogInformation($"Successfully cloned workbook with options and ID: {result.Value.Id}", result.Value.Id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error cloning workbook with options and ID: {workbook.Id}", ex, workbook.Id);
                return Result<IWorkbookDomain>.Failure($"Unexpected error cloning workbook: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Validates that a file is a valid Excel workbook
        /// </summary>
        /// <param name="filePath">The file path to validate</param>
        /// <returns>Result containing validation results or error information</returns>
        public Result<bool> IsValidWorkbook(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return Result<bool>.Failure("File path cannot be null or empty");
            }

            try
            {
                _logger.LogWorkbookOperation($"Validating workbook file: {filePath}", Guid.Empty);
                
                var result = _engine.IsValidWorkbook(filePath);
                if (result.IsSuccess)
                {
                    _logger.LogInformation($"Workbook validation result for '{filePath}': {result.Value}", Guid.Empty);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error validating workbook file: {filePath}", ex, Guid.Empty);
                return Result<bool>.Failure($"Unexpected error validating workbook: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Validates that a stream contains a valid Excel workbook
        /// </summary>
        /// <param name="stream">The stream to validate</param>
        /// <returns>Result containing validation results or error information</returns>
        public Result<bool> IsValidWorkbook(Stream stream)
        {
            if (stream == null)
            {
                return Result<bool>.Failure("Stream cannot be null");
            }

            try
            {
                _logger.LogWorkbookOperation("Validating workbook stream", Guid.Empty);
                
                var result = _engine.IsValidWorkbook(stream);
                if (result.IsSuccess)
                {
                    _logger.LogInformation($"Workbook validation result for stream: {result.Value}", Guid.Empty);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error validating workbook stream", ex, Guid.Empty);
                return Result<bool>.Failure($"Unexpected error validating workbook: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets information about a workbook file without opening it
        /// </summary>
        /// <param name="filePath">The file path to the workbook</param>
        /// <returns>Result containing workbook information or error information</returns>
        public Result<WorkbookInfo> GetWorkbookInfo(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return Result<WorkbookInfo>.Failure("File path cannot be null or empty");
            }

            try
            {
                _logger.LogWorkbookOperation($"Getting workbook info for file: {filePath}", Guid.Empty);
                
                var result = _engine.GetWorkbookInfo(filePath);
                if (result.IsSuccess)
                {
                    _logger.LogInformation($"Successfully retrieved workbook info for '{filePath}'", Guid.Empty);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error getting workbook info for file: {filePath}", ex, Guid.Empty);
                return Result<WorkbookInfo>.Failure($"Unexpected error getting workbook info: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets information about a workbook stream without opening it
        /// </summary>
        /// <param name="stream">The stream containing the workbook data</param>
        /// <returns>Result containing workbook information or error information</returns>
        public Result<WorkbookInfo> GetWorkbookInfo(Stream stream)
        {
            if (stream == null)
            {
                return Result<WorkbookInfo>.Failure("Stream cannot be null");
            }

            try
            {
                _logger.LogWorkbookOperation("Getting workbook info for stream", Guid.Empty);
                
                var result = _engine.GetWorkbookInfo(stream);
                if (result.IsSuccess)
                {
                    _logger.LogInformation("Successfully retrieved workbook info for stream", Guid.Empty);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error getting workbook info for stream", ex, Guid.Empty);
                return Result<WorkbookInfo>.Failure($"Unexpected error getting workbook info: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Creates a workbook domain object from an engine instance
        /// </summary>
        /// <param name="engineInstance">The engine instance</param>
        /// <returns>Result containing the workbook domain or error information</returns>
        private Result<IWorkbookDomain> CreateWorkbookFromEngine(IWorkbookEngineInstance engineInstance)
        {
            try
            {
                // Create adapters for the engine instance
                var contentAdapter = new WorkbookContentAdapter(engineInstance.Workbook as IWorkbook);
                var metadataAdapter = new WorkbookMetadataAdapter(engineInstance);

                // Create the domain object
                var workbook = new WorkbookDomain(contentAdapter, metadataAdapter);

                // Validate the workbook
                var validationResult = _validator.ValidateWorkbook(workbook);
                if (!validationResult.IsSuccess)
                {
                    _logger.LogWarning($"Workbook validation failed: {validationResult.Error}", workbook.Id);
                    // Continue anyway - validation warnings don't prevent creation
                }

                return Result<IWorkbookDomain>.Success(workbook);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error creating workbook from engine instance", ex, Guid.Empty);
                return Result<IWorkbookDomain>.Failure($"Error creating workbook from engine: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Applies metadata to a workbook
        /// </summary>
        /// <param name="workbook">The workbook to apply metadata to</param>
        /// <param name="metadata">The metadata to apply</param>
        private void ApplyMetadataToWorkbook(IWorkbookDomain workbook, WorkbookCreationMetadata metadata)
        {
            if (!string.IsNullOrWhiteSpace(metadata.Title))
                workbook.Title = metadata.Title;
            
            if (!string.IsNullOrWhiteSpace(metadata.Author))
                workbook.Author = metadata.Author;
            
            if (!string.IsNullOrWhiteSpace(metadata.Subject))
                workbook.Subject = metadata.Subject;
            
            if (!string.IsNullOrWhiteSpace(metadata.Company))
                workbook.Company = metadata.Company;
            
            if (!string.IsNullOrWhiteSpace(metadata.Category))
                workbook.Category = metadata.Category;
            
            if (!string.IsNullOrWhiteSpace(metadata.Keywords))
                workbook.Keywords = metadata.Keywords;
            
            if (!string.IsNullOrWhiteSpace(metadata.Comments))
                workbook.Comments = metadata.Comments;
            
            if (!string.IsNullOrWhiteSpace(metadata.Manager))
                workbook.Manager = metadata.Manager;
            
            if (!string.IsNullOrWhiteSpace(metadata.Application))
                workbook.Application = metadata.Application;
            
            if (!string.IsNullOrWhiteSpace(metadata.ApplicationVersion))
                workbook.ApplicationVersion = metadata.ApplicationVersion;
            
            if (!string.IsNullOrWhiteSpace(metadata.Template))
                workbook.Template = metadata.Template;
        }
    }
}
