# Plan Command

## Purpose
Create comprehensive, structured plans for any development task, feature, or project using systematic planning methodologies that align with Cursor's recommended approach for large-scale implementations.

## When to Use
- **Feature Planning**: When designing new features or major functionality
- **Project Planning**: When starting new projects or major refactoring efforts
- **Task Decomposition**: When breaking down complex tasks into manageable steps
- **Architecture Planning**: When designing system architecture or major structural changes
- **Migration Planning**: When planning technology migrations or upgrades
- **Bug Fix Planning**: When planning complex bug fixes or system improvements

## Command Structure

### Phase 1: Requirements Analysis & Scope Definition
**AUTONOMOUS PRINCIPAL ENGINEER DOCTRINE**: Begin with comprehensive understanding of requirements and constraints.

#### Requirements Gathering
- **Functional Requirements**: What the system must do
- **Non-Functional Requirements**: Performance, security, scalability, maintainability
- **Business Requirements**: Value proposition, success metrics, stakeholder needs
- **Technical Requirements**: Technology constraints, integration needs, performance targets
- **User Requirements**: User experience, accessibility, usability needs

#### Existing Project Pattern Analysis
- **Code Pattern Analysis**: Analyze existing code patterns and architectural decisions
- **Testing Pattern Analysis**: Review existing testing approaches and coverage
- **Design Pattern Analysis**: Identify existing design patterns and conventions
- **Architecture Pattern Analysis**: Understand current architectural patterns
- **Quality Standard Analysis**: Assess current quality standards and practices

#### Pattern Alignment Decision
- **Clean Architecture Alignment**: Determine if existing patterns align with clean architecture
- **Clean Code Alignment**: Assess if existing patterns follow clean code principles
- **SOLID Principles Alignment**: Evaluate adherence to SOLID principles
- **Testing Standard Alignment**: Check if existing testing meets coverage requirements
- **Decision Point**: Ask if planning should follow clean architecture/clean code/SOLID principles or existing project patterns

#### Scope Definition
- **In Scope**: Clearly defined deliverables and features
- **Out of Scope**: Explicitly excluded items and future considerations
- **Dependencies**: External dependencies and prerequisites
- **Constraints**: Time, budget, resource, and technical constraints
- **Assumptions**: Key assumptions that affect the plan

### Phase 2: Architecture & Design Planning
**Technology-Adaptive Protocols**: Design architecture appropriate to the technology stack and requirements using clean architecture, clean code, and SOLID principles.

#### Clean Architecture Planning
- **Layered Architecture**: Plan for clear separation of concerns across layers
- **Dependency Inversion**: Design with dependencies pointing inward toward core business logic
- **Interface Segregation**: Define focused, minimal interfaces for each component
- **Single Responsibility**: Ensure each component has one clear responsibility
- **Open/Closed Principle**: Design components to be open for extension, closed for modification

#### Clean Code Planning
- **Readability Focus**: Plan for highly readable, self-documenting code
- **Maintainability**: Design for easy maintenance and future modifications
- **Testability**: Ensure all components can be easily unit tested
- **Practicality**: Balance theoretical principles with practical implementation needs
- **Naming Conventions**: Plan for clear, descriptive naming throughout

#### SOLID Principles Integration
- **Single Responsibility Principle**: Each class/component has one reason to change
- **Open/Closed Principle**: Components are open for extension, closed for modification
- **Liskov Substitution Principle**: Derived classes must be substitutable for base classes
- **Interface Segregation Principle**: Clients should not depend on interfaces they don't use
- **Dependency Inversion Principle**: Depend on abstractions, not concretions

#### System Architecture Design
- **Component Architecture**: Identify major components and their responsibilities
- **Data Architecture**: Design data models, storage, and flow patterns
- **Integration Architecture**: Define external integrations and APIs
- **Security Architecture**: Design security patterns and access controls
- **Performance Architecture**: Plan for scalability and performance requirements

#### Technical Design
- **Technology Stack Selection**: Choose appropriate technologies and frameworks
- **Design Patterns**: Identify and apply relevant design patterns
- **API Design**: Design internal and external APIs
- **Database Design**: Plan data storage and access patterns
- **UI/UX Design**: Plan user interface and experience

### Phase 3: Task Decomposition & Sequencing
**Incremental Development Approach**: Break down work into small, manageable, well-isolated tasks following Cursor's recommendation for "small, well-isolated steps."

#### Small Sub-Task Chunking Strategy
- **Atomic Task Creation**: Break each feature into atomic, single-responsibility tasks
- **2-4 Hour Chunks**: Create tasks that can be completed in 2-4 hour sessions
- **Well-Isolated Steps**: Ensure each task is independent and can be developed separately
- **Single Validation Point**: Each task should have one clear validation checkpoint
- **Minimal Dependencies**: Minimize dependencies between tasks to enable parallel work

#### Task Breakdown Hierarchy
- **Epic Level**: Large features or major functionality (1-2 weeks)
- **Feature Level**: Specific features or components (2-5 days)
- **Task Level**: Individual development tasks (4-8 hours)
- **Sub-Task Level**: Small, atomic implementation steps (1-4 hours)
- **Validation Level**: Testing and validation checkpoints (1-2 hours)

#### Small Sub-Task Examples
```markdown
## Example: User Authentication Feature

### Epic: User Authentication System
- **Feature 1**: OAuth2 Integration
  - **Task 1.1**: Set up OAuth2 provider configuration (2 hours)
  - **Task 1.2**: Implement OAuth2 client setup (2 hours)
  - **Task 1.3**: Create OAuth2 callback handler (3 hours)
  - **Task 1.4**: Add OAuth2 error handling (2 hours)
  - **Task 1.5**: Test OAuth2 integration (2 hours)

- **Feature 2**: User Management
  - **Task 2.1**: Create user data model (2 hours)
  - **Task 2.2**: Implement user registration (3 hours)
  - **Task 2.3**: Implement user login (2 hours)
  - **Task 2.4**: Add user profile management (4 hours)
  - **Task 2.5**: Test user management features (2 hours)

- **Feature 3**: Security Features
  - **Task 3.1**: Implement JWT token generation (2 hours)
  - **Task 3.2**: Add token validation middleware (2 hours)
  - **Task 3.3**: Implement password hashing (1 hour)
  - **Task 3.4**: Add rate limiting (2 hours)
  - **Task 3.5**: Test security features (2 hours)
```

#### Task Organization
- **Priority Ordering**: Rank tasks by business value and technical dependencies
- **Sprint Planning**: Organize tasks into development sprints or iterations
- **Parallel Work**: Identify tasks that can be worked on simultaneously
- **Sequential Dependencies**: Ensure proper ordering of dependent tasks
- **Milestone Definition**: Create clear milestones and deliverables

#### Task Chunking Validation
- **Size Validation**: Ensure each task is 1-4 hours of work
- **Isolation Validation**: Verify each task can be developed independently
- **Dependency Validation**: Minimize dependencies between tasks
- **Testability Validation**: Ensure each task can be tested in isolation
- **Completion Validation**: Each task should have a clear completion criteria

#### Task Chunking Anti-Patterns
- **FORBIDDEN**: Tasks larger than 8 hours (break down further)
- **FORBIDDEN**: Tasks with multiple responsibilities (split into separate tasks)
- **FORBIDDEN**: Tasks with complex dependencies (simplify or reorder)
- **FORBIDDEN**: Tasks without clear completion criteria (define success metrics)
- **FORBIDDEN**: Tasks that cannot be tested independently (refactor approach)

#### Quality Standards Integration
- **Clean Code Requirements**: Each task must follow clean code principles
- **SOLID Principles**: Each task must adhere to SOLID principles
- **Testability Requirements**: Each task must be easily testable
- **Readability Requirements**: Each task must produce readable, maintainable code
- **Coverage Requirements**: Each task must include unit tests with 90% coverage

### Phase 4: Implementation Strategy Planning
**Structured Execution Planning**: Plan the execution approach using Cursor's recommended methodologies.

#### Development Approach
- **Modular Implementation**: Plan for small, isolated, testable modules
- **Incremental Validation**: Plan for continuous testing and validation
- **Iterative Development**: Plan for iterative refinement and improvement
- **Risk Mitigation**: Plan for handling technical and business risks
- **Quality Assurance**: Plan for code quality and testing strategies

#### Quality Standards Enforcement
- **Clean Architecture**: Enforce layered architecture with dependency inversion
- **Clean Code**: Enforce readable, maintainable, and self-documenting code
- **SOLID Principles**: Enforce all five SOLID principles throughout development
- **Testability**: Ensure all code is easily testable and maintainable
- **Coverage Requirements**: Enforce 90% unit test coverage and 20-30% E2E coverage

#### Technology-Specific Planning
- **Framework Patterns**: Plan for framework-specific best practices
- **Build System Planning**: Plan for build, test, and deployment processes
- **Configuration Management**: Plan for environment and configuration management
- **Monitoring & Observability**: Plan for logging, monitoring, and debugging
- **Documentation Planning**: Plan for technical and user documentation

### Phase 5: Testing Strategy & Quality Assurance
**Comprehensive Testing Requirements**: Plan mandatory testing with specific coverage requirements and quality standards.

#### Mandatory Testing Requirements
- **Unit Testing**: MANDATORY with 90% coverage by default
  - **Component Testing**: Test individual components in isolation
  - **Function Testing**: Test individual functions and methods
  - **Mock Testing**: Use mocks for external dependencies
  - **Edge Case Testing**: Test boundary conditions and error cases
  - **Coverage Validation**: Ensure 90% code coverage minimum

- **End-to-End Testing**: MANDATORY with 20-30% coverage by default
  - **User Workflow Testing**: Test complete user journeys
  - **Integration Testing**: Test system integration points
  - **API Testing**: Test external API integrations
  - **Cross-Browser Testing**: Test across different browsers
  - **Coverage Validation**: Ensure 20-30% E2E coverage minimum

#### Testing Strategy
- **Test-Driven Development**: Plan for TDD approach where applicable
- **Behavior-Driven Development**: Plan for BDD for user-facing features
- **Integration Testing**: Plan for system integration and API testing
- **Performance Testing**: Plan for load, stress, and performance testing
- **Security Testing**: Plan for security vulnerability and penetration testing

#### Quality Gates
- **Code Quality**: Plan for code review, linting, and quality metrics
- **Test Coverage**: Plan for automated coverage reporting and validation
- **Performance Gates**: Plan for performance benchmarks and monitoring
- **Security Gates**: Plan for security reviews and vulnerability scanning
- **User Acceptance**: Plan for user testing and acceptance criteria
- **Deployment Gates**: Plan for deployment validation and rollback procedures

### Phase 6: Status Tracking & Handoff Planning
**Multi-Session Continuity**: Plan for tracking implementation status and enabling seamless handoff between LLM sessions.

#### Status Tracking Strategy
- **Implementation Status**: Plan for tracking completion status of each task
- **Progress Monitoring**: Plan for monitoring progress against milestones
- **Quality Gates**: Plan for tracking quality gate completion status
- **Issue Tracking**: Plan for tracking and resolving implementation issues
- **Dependency Status**: Plan for tracking dependency completion and readiness

#### Handoff Documentation
- **Session Handoff**: Plan for documenting work completed in each session
- **Context Preservation**: Plan for preserving context and decision rationale
- **Next Steps**: Plan for clearly defining next steps for subsequent sessions
- **Blockers and Issues**: Plan for documenting blockers and unresolved issues
- **Knowledge Transfer**: Plan for transferring session-specific knowledge

#### Status Tracking Artifacts
- **Implementation Log**: Detailed log of what was implemented in each session
- **Progress Report**: Current status of all tasks and milestones
- **Quality Status**: Status of all quality gates and validation checkpoints
- **Issue Log**: Log of all issues, blockers, and resolutions
- **Context Summary**: Summary of current context and state

### Phase 7: Documentation & Communication Planning
**Comprehensive Documentation**: Plan for maintaining clear documentation and communication.

#### Documentation Strategy
- **Technical Documentation**: Plan for API docs, architecture docs, and code documentation
- **User Documentation**: Plan for user guides, help content, and training materials
- **Process Documentation**: Plan for development processes and procedures
- **Decision Documentation**: Plan for architectural decisions and rationale
- **Maintenance Documentation**: Plan for ongoing maintenance and support

#### Communication Planning
- **Stakeholder Communication**: Plan for regular updates and progress reports
- **Team Communication**: Plan for team coordination and knowledge sharing
- **Client Communication**: Plan for client updates and feedback collection
- **Change Management**: Plan for communicating changes and managing expectations
- **Knowledge Transfer**: Plan for transferring knowledge and expertise

## Task Chunking Methodologies

### Small Sub-Task Chunking
- **Atomic Decomposition**: Break features into atomic, single-responsibility tasks
- **Time-Boxed Tasks**: Create tasks that can be completed in 1-4 hour sessions
- **Well-Isolated Steps**: Ensure each task is independent and testable
- **Minimal Dependencies**: Minimize dependencies to enable parallel work
- **Clear Completion Criteria**: Each task has unambiguous success metrics

### Hierarchical Task Breakdown
- **Epic Level**: Large features or major functionality (1-2 weeks)
- **Feature Level**: Specific features or components (2-5 days)
- **Task Level**: Individual development tasks (4-8 hours)
- **Sub-Task Level**: Small, atomic implementation steps (1-4 hours)
- **Validation Level**: Testing and validation checkpoints (1-2 hours)

### Task Chunking Best Practices
- **Single Responsibility**: Each task has one clear purpose
- **Testable Units**: Each task can be tested independently
- **Incremental Value**: Each task delivers incremental value
- **Parallel Execution**: Tasks can be worked on simultaneously
- **Clear Dependencies**: Dependencies are explicit and minimal

## Planning Methodologies

### Agile Planning
- **Sprint Planning**: Break work into 1-2 week sprints
- **User Story Mapping**: Organize features around user journeys
- **Backlog Management**: Maintain prioritized backlog of work
- **Retrospective Planning**: Plan for continuous improvement
- **Velocity Tracking**: Monitor team capacity and delivery speed

### Waterfall Planning
- **Phase-Based Planning**: Organize work into distinct phases
- **Milestone-Driven**: Focus on major deliverables and milestones
- **Documentation-Heavy**: Emphasize comprehensive documentation
- **Change Control**: Manage scope changes through formal processes
- **Quality Gates**: Define strict quality criteria for each phase

### Hybrid Planning
- **Adaptive Planning**: Combine agile and waterfall approaches as needed
- **Risk-Based Planning**: Adjust approach based on risk assessment
- **Stakeholder-Driven**: Adapt to stakeholder needs and preferences
- **Technology-Adaptive**: Adjust methodology based on technology stack
- **Context-Sensitive**: Adapt planning approach to project context

## Planning Outputs

### Project Plan Document
- **Executive Summary**: High-level overview for stakeholders
- **Detailed Requirements**: Comprehensive requirements specification
- **Architecture Overview**: System architecture and design decisions
- **Implementation Timeline**: Detailed schedule with milestones
- **Resource Requirements**: Team, tools, and infrastructure needs
- **Risk Assessment**: Identified risks and mitigation strategies
- **Success Criteria**: Clear definition of project success

### Task Chunking Plan
- **Small Sub-Task Breakdown**: Detailed breakdown into 1-4 hour tasks
- **Task Dependencies**: Clear mapping of task dependencies and critical path
- **Parallel Work Opportunities**: Identification of tasks that can be worked on simultaneously
- **Validation Checkpoints**: Clear validation points for each task
- **Completion Criteria**: Unambiguous success metrics for each task
- **Time Estimates**: Realistic time estimates for each task chunk

### Technical Specifications
- **System Architecture**: Detailed technical architecture
- **API Specifications**: Complete API documentation
- **Database Schema**: Data model and storage design
- **Security Specifications**: Security requirements and implementation
- **Performance Specifications**: Performance requirements and benchmarks
- **Integration Specifications**: External integration requirements

### Implementation Roadmap
- **Phase Breakdown**: Detailed phases with deliverables
- **Task List**: Comprehensive list of all tasks
- **Dependency Map**: Task dependencies and critical path
- **Resource Allocation**: Team assignments and responsibilities
- **Timeline**: Detailed schedule with dates and milestones
- **Risk Mitigation**: Specific actions to address identified risks

## Quality Standards & Requirements

### MANDATORY Quality Standards
**Every plan MUST enforce these quality standards unless explicitly overridden by existing project patterns.**

#### Clean Architecture Requirements
- **Layered Architecture**: Clear separation of concerns across layers
- **Dependency Inversion**: Dependencies point inward toward core business logic
- **Interface Segregation**: Focused, minimal interfaces for each component
- **Single Responsibility**: Each component has one clear responsibility
- **Open/Closed Principle**: Components open for extension, closed for modification

#### Clean Code Requirements
- **Readability**: Highly readable, self-documenting code
- **Maintainability**: Easy maintenance and future modifications
- **Testability**: All components easily unit tested
- **Practicality**: Balance theoretical principles with practical needs
- **Naming Conventions**: Clear, descriptive naming throughout

#### SOLID Principles Requirements
- **Single Responsibility Principle**: Each class/component has one reason to change
- **Open/Closed Principle**: Components open for extension, closed for modification
- **Liskov Substitution Principle**: Derived classes substitutable for base classes
- **Interface Segregation Principle**: Clients don't depend on unused interfaces
- **Dependency Inversion Principle**: Depend on abstractions, not concretions

#### Testing Requirements
- **Unit Testing**: MANDATORY with 90% coverage by default
- **End-to-End Testing**: MANDATORY with 20-30% coverage by default
- **Test-Driven Development**: TDD approach where applicable
- **Behavior-Driven Development**: BDD for user-facing features
- **Coverage Validation**: Automated coverage reporting and validation

### Existing Pattern Analysis & Decision Making
**When planning for existing projects, analyze current patterns and make informed decisions.**

#### Pattern Analysis Process
1. **Code Pattern Analysis**: Analyze existing code patterns and architectural decisions
2. **Testing Pattern Analysis**: Review existing testing approaches and coverage
3. **Design Pattern Analysis**: Identify existing design patterns and conventions
4. **Architecture Pattern Analysis**: Understand current architectural patterns
5. **Quality Standard Analysis**: Assess current quality standards and practices

#### Decision Making Framework
- **Alignment Assessment**: Determine if existing patterns align with clean architecture/clean code/SOLID principles
- **Coverage Assessment**: Check if existing testing meets coverage requirements
- **Quality Assessment**: Evaluate current quality standards and practices
- **Decision Point**: Ask if planning should follow clean architecture/clean code/SOLID principles or existing project patterns
- **Documentation**: Document the decision and rationale for future reference

## Usage Examples

### Feature Planning
```
/plan
Create a comprehensive plan for implementing user authentication with OAuth2, including frontend components, backend APIs, database schema, and security considerations.
```

### Project Planning
```
/plan
Develop a complete project plan for migrating our Angular application to React, including architecture design, migration strategy, timeline, and risk assessment.
```

### Architecture Planning
```
/plan
Design a microservices architecture for our e-commerce platform, including service boundaries, communication patterns, data management, and deployment strategy.
```

### Bug Fix Planning
```
/plan
Create a detailed plan for fixing the memory leak in our React application, including root cause analysis, implementation approach, testing strategy, and prevention measures.
```

### Existing Project Planning
```
/plan
Plan the implementation of a new feature in an existing project. Analyze current patterns and determine if we should follow clean architecture principles or existing project patterns.
```

## Success Criteria

### Planning Quality
- **Comprehensive**: All aspects of the project are covered
- **Realistic**: Plans are achievable with available resources
- **Detailed**: Sufficient detail for implementation
- **Flexible**: Plans can adapt to changing requirements
- **Measurable**: Clear success criteria and metrics

### Implementation Readiness
- **Clear Requirements**: All requirements are well-defined
- **Technical Clarity**: Technical approach is clear and feasible
- **Resource Allocation**: Resources are properly allocated
- **Risk Management**: Risks are identified and mitigated
- **Quality Assurance**: Quality measures are in place

## Integration with Other Commands

### Pre-Implementation
- Use before `/do` to ensure comprehensive planning before execution
- Use before `/feature-implement` to plan feature development
- Use before `/refactor` to plan refactoring efforts

### Analysis Support
- Use with `/analyze-project-deep` to understand current state before planning
- Use with `/analyze-code-deep` to understand existing code before planning changes
- Use with `/understand` to build understanding before planning

### Execution Support
- Use with `/do` to execute planned work
- Use with `/test` to implement planned testing strategies
- Use with `/debug` to plan debugging and troubleshooting approaches

**Remember**: The `/plan` command is your foundation for successful project execution. Use it to create comprehensive, structured plans that follow Cursor's recommended approach for large-scale implementations. The command adapts to your specific needs and provides the level of planning detail appropriate for your context.
