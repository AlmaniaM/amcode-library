# Understand Project Command

## Purpose
Comprehensively understand every aspect of a software project to create a complete context that other LLMs can consume without wasting context on repeated analysis. This command provides deep, structured understanding of the entire project ecosystem.

## When to Use
- **Project Onboarding**: When new team members or LLMs need complete project understanding
- **Context Preservation**: Before major refactoring or architectural changes
- **Multi-LLM Coordination**: When multiple AI agents need to work on the same project
- **Project Documentation**: Creating comprehensive project documentation
- **Technical Due Diligence**: Understanding project before acquisition or handoff
- **Migration Planning**: Understanding current state before major migrations
- **Code Review Preparation**: Understanding project before conducting reviews

## Command Structure

### Phase 1: Project Discovery & Technology Stack Analysis
**AUTONOMOUS PRINCIPAL ENGINEER DOCTRINE**: Begin with comprehensive project reconnaissance to understand the complete technology ecosystem.

#### Project Type Identification
- **Application Type**: Web app, mobile app, desktop app, API, library, monorepo, microservices
- **Architecture Pattern**: Monolithic, microservices, serverless, JAMstack, hybrid
- **Deployment Model**: Cloud, on-premise, hybrid, edge computing
- **Development Model**: Solo, team, open source, enterprise

#### Technology Stack Detection
- **Primary Languages**: JavaScript/TypeScript, Python, Java, C#, Go, Rust, etc.
- **Frontend Framework**: React, Vue, Angular, Svelte, vanilla JS, etc.
- **Backend Framework**: Express, FastAPI, Spring, ASP.NET, Django, etc.
- **Database Technologies**: PostgreSQL, MongoDB, Redis, MySQL, etc.
- **Build Tools**: Webpack, Vite, Rollup, esbuild, Parcel, etc.
- **Testing Frameworks**: Jest, Vitest, Cypress, Playwright, etc.
- **Deployment Tools**: Docker, Kubernetes, Vercel, AWS, etc.

#### Configuration Analysis
- **Package Management**: npm, yarn, pnpm, pip, cargo, etc.
- **Environment Configuration**: .env files, config files, environment variables
- **Build Configuration**: webpack.config.js, vite.config.js, tsconfig.json, etc.
- **Linting & Formatting**: ESLint, Prettier, Stylelint, etc.
- **CI/CD Configuration**: GitHub Actions, GitLab CI, Jenkins, etc.

### Phase 2: Dependency & Infrastructure Analysis
**Technology-Adaptive Protocols**: Deep analysis of all project dependencies and infrastructure.

#### Dependency Analysis
- **Production Dependencies**: Core libraries and frameworks used in production
- **Development Dependencies**: Build tools, testing frameworks, development utilities
- **Peer Dependencies**: Required peer packages and version constraints
- **Optional Dependencies**: Optional packages that enhance functionality
- **Dependency Tree**: Complete dependency hierarchy and relationships
- **Version Analysis**: Current versions, latest versions, security vulnerabilities
- **License Analysis**: Open source licenses and compliance requirements

#### Infrastructure Understanding
- **Containerization**: Dockerfile analysis, container configuration, multi-stage builds
- **Orchestration**: Kubernetes manifests, Docker Compose, service definitions
- **Cloud Services**: AWS, Azure, GCP services and configurations
- **CDN & Static Assets**: Asset optimization, CDN configuration, caching strategies
- **Monitoring & Logging**: APM tools, logging frameworks, error tracking
- **Security Infrastructure**: Authentication, authorization, encryption, secrets management

### Phase 3: Source Code Architecture Analysis
**Universal Frontend Stewardship**: Deep understanding of the entire codebase structure and patterns.

#### Directory Structure Analysis
- **Root Level**: Configuration files, documentation, scripts, assets
- **Source Directory**: Main application code organization
- **Component Architecture**: Component hierarchy, patterns, and relationships
- **Module Organization**: Service modules, utility modules, feature modules
- **Asset Organization**: Images, fonts, styles, static files
- **Test Organization**: Unit tests, integration tests, e2e tests structure

#### Code Pattern Analysis
- **Design Patterns**: MVC, MVP, MVVM, Observer, Factory, Singleton, etc.
- **Architecture Patterns**: Layered, hexagonal, clean architecture, DDD
- **State Management**: Redux, Zustand, Context API, MobX, etc.
- **Data Flow**: Unidirectional, bidirectional, event-driven, reactive
- **Error Handling**: Try-catch patterns, error boundaries, logging strategies
- **Performance Patterns**: Lazy loading, memoization, virtualization, caching

#### Component & Module Deep Dive
- **Component Analysis**:
  - Component hierarchy and relationships
  - Props interfaces and data flow
  - State management and lifecycle
  - Styling approaches and theming
  - Accessibility implementation
  - Performance optimizations

- **Service Layer Analysis**:
  - API service patterns and abstractions
  - Data transformation and validation
  - Caching strategies and state management
  - Error handling and retry logic
  - Authentication and authorization flows

- **Utility & Helper Analysis**:
  - Common utility functions and patterns
  - Data transformation helpers
  - Validation schemas and rules
  - Constants and configuration management
  - Type definitions and interfaces

### Phase 4: Testing Strategy & Quality Analysis
**Multi-Layer Verification**: Comprehensive understanding of testing approaches and quality measures.

#### Testing Framework Analysis
- **Unit Testing**: Framework, patterns, coverage, mocking strategies
- **Integration Testing**: API testing, database testing, service integration
- **End-to-End Testing**: User journey testing, cross-browser testing
- **Visual Testing**: Screenshot testing, visual regression testing
- **Performance Testing**: Load testing, stress testing, benchmarking
- **Security Testing**: Vulnerability scanning, penetration testing

#### Test Organization & Patterns
- **Test Structure**: Test file organization, naming conventions
- **Test Data Management**: Fixtures, factories, test databases
- **Mocking Strategies**: Service mocking, API mocking, dependency injection
- **Test Utilities**: Custom matchers, helpers, test runners
- **CI/CD Integration**: Automated testing, test reporting, coverage tracking

#### Quality Metrics & Standards
- **Code Quality**: Linting rules, complexity metrics, maintainability
- **Performance Metrics**: Bundle size, runtime performance, Core Web Vitals
- **Security Standards**: OWASP compliance, vulnerability scanning
- **Accessibility Standards**: WCAG compliance, screen reader testing
- **Documentation Quality**: Code comments, README files, API documentation

### Phase 5: Build & Deployment Pipeline Analysis
**Comprehensive Build Understanding**: Deep analysis of all build processes and deployment strategies.

#### Build Process Analysis
- **Build Configuration**: Webpack, Vite, Rollup, or other bundler configuration
- **Compilation Process**: TypeScript compilation, Babel transpilation, CSS processing
- **Asset Processing**: Image optimization, font loading, static asset handling
- **Code Splitting**: Route-based splitting, component-based splitting, vendor splitting
- **Environment Handling**: Development, staging, production configurations

#### Custom Scripts & Tools
- **Package.json Scripts**: Build, test, lint, format, deploy scripts
- **Custom Build Tools**: Webpack plugins, Rollup plugins, custom transformers
- **Development Tools**: Hot reloading, dev servers, debugging tools
- **Code Generation**: CLI tools, scaffolding, boilerplate generation
- **Migration Tools**: Database migrations, data transformations, schema updates

#### Deployment Pipeline Analysis
- **Deployment Targets**: Production, staging, development environments
- **Deployment Strategies**: Blue-green, rolling, canary, feature flags
- **Environment Configuration**: Environment variables, secrets management
- **Health Checks**: Application health, dependency health, monitoring
- **Rollback Strategies**: Automated rollback, manual rollback, data recovery

### Phase 6: Documentation & Knowledge Analysis
**Comprehensive Documentation Understanding**: Analysis of all project documentation and knowledge artifacts.

#### Documentation Analysis
- **README Files**: Project overview, setup instructions, contribution guidelines
- **API Documentation**: OpenAPI specs, GraphQL schemas, endpoint documentation
- **Architecture Documentation**: System design, component diagrams, data flow
- **Deployment Documentation**: Infrastructure setup, deployment procedures
- **Development Documentation**: Coding standards, development workflow, testing guidelines

#### Knowledge Artifacts
- **Decision Records**: ADRs (Architecture Decision Records), design decisions
- **Changelog**: Version history, feature additions, bug fixes, breaking changes
- **Migration Guides**: Framework upgrades, breaking changes, data migrations
- **Troubleshooting Guides**: Common issues, debugging procedures, FAQ
- **Performance Guides**: Optimization strategies, monitoring procedures

### Phase 7: Context Synthesis & Communication
**Comprehensive Context Creation**: Synthesize all understanding into a consumable context for other LLMs.

#### Context Structure
- **Executive Summary**: High-level project overview for stakeholders
- **Technical Summary**: Detailed technical overview for developers
- **Architecture Overview**: System architecture and component relationships
- **Technology Stack Summary**: Complete technology inventory
- **Development Workflow**: How to work with the project
- **Key Patterns & Conventions**: Important patterns and coding standards
- **Critical Dependencies**: Important external dependencies and their purposes
- **Testing Strategy**: How testing is organized and executed
- **Deployment Process**: How the project is built and deployed
- **Known Issues & Limitations**: Current problems and constraints
- **Future Considerations**: Planned changes and technical debt

#### Context Optimization
- **Hierarchical Organization**: Information organized by importance and detail level
- **Cross-References**: Links between related concepts and components
- **Quick Reference**: Fast lookup for common tasks and information
- **Decision Context**: Why certain decisions were made and their implications
- **Actionable Information**: Clear next steps and recommendations

## Universal Project Understanding Protocols

### Technology-Adaptive Analysis
- **Framework-Specific Patterns**: Adapt analysis to specific frameworks and patterns
- **Language-Specific Conventions**: Apply language-specific best practices and conventions
- **Architecture-Specific Analysis**: Tailor analysis to architectural patterns and principles
- **Industry-Specific Standards**: Apply relevant industry standards and compliance requirements

### Complexity Handling
- **Large Projects**: Structured breakdown with clear organization and navigation
- **Legacy Projects**: Historical context and migration considerations
- **Multi-Repository Projects**: Cross-repository dependencies and relationships
- **Microservices Projects**: Service boundaries and communication patterns

### Quality Assurance
- **Completeness Verification**: Ensure all important aspects are covered
- **Accuracy Validation**: Verify understanding matches actual project state
- **Consistency Check**: Ensure information is internally consistent
- **Gap Identification**: Identify areas that need deeper investigation
- **Stakeholder Alignment**: Verify understanding matches team expectations

## Usage Examples

### Complete Project Understanding
```
/understand-project
Provide a comprehensive understanding of this entire project for new team members.
```

### Technology Stack Focus
```
/understand-project
Focus on understanding the technology stack, dependencies, and build process.
```

### Architecture Deep Dive
```
/understand-project
Deep dive into the source code architecture, patterns, and component relationships.
```

### Testing & Quality Analysis
```
/understand-project
Comprehensive analysis of testing strategies, quality metrics, and CI/CD pipeline.
```

## Success Criteria

### Understanding Completeness
- **Technology Stack**: 100% of technologies and tools identified and understood
- **Dependencies**: Complete dependency tree with versions and purposes
- **Architecture**: Full understanding of system architecture and patterns
- **Testing**: Complete testing strategy and quality measures understood
- **Deployment**: Full understanding of build and deployment processes
- **Documentation**: All project documentation and knowledge artifacts analyzed

### Context Quality
- **Comprehensive**: All important project aspects are covered
- **Accurate**: Understanding correctly represents the actual project
- **Structured**: Information is well-organized and easy to navigate
- **Actionable**: Provides clear guidance for working with the project
- **Efficient**: Optimized for LLM consumption without context waste

### Communication Effectiveness
- **Multi-Level Detail**: Appropriate detail for different audiences
- **Visual Clarity**: Uses diagrams and examples when helpful
- **Quick Reference**: Easy lookup for common tasks and information
- **Decision Context**: Explains why decisions were made
- **Future-Oriented**: Considers planned changes and technical debt

## Integration with Other Commands

### Pre-Implementation
- Use before `/do` to ensure complete project understanding
- Use before `/feature-implement` to understand project context
- Use before `/refactor` to understand current architecture
- Use before `/migrate` to understand migration context

### Analysis Support
- Use with `/analyze-project-deep` for comprehensive analysis
- Use with `/debug` to understand problem context
- Use with `/test` to understand testing approach
- Use with `/optimize` to understand performance context

### Team Coordination
- Use for onboarding new team members
- Use for handoff between teams
- Use for technical due diligence
- Use for project documentation

**Remember**: The `/understand-project` command is your comprehensive project analysis tool. It provides complete understanding of every aspect of a project, creating a rich context that other LLMs can consume efficiently without repeated analysis. Use it whenever you need deep, structured understanding of an entire project ecosystem.

