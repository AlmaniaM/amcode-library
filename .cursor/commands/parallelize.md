# Parallelize Command

## Purpose
Break down a single comprehensive plan into independent, parallel-executable phases that can be tackled by multiple LLMs simultaneously, ensuring no phase dependencies and optimal parallel execution.

## When to Use
- **Large-Scale Projects**: When a single plan is too large for one LLM to handle efficiently
- **Multi-Agent Development**: When multiple LLMs need to work on the same project simultaneously
- **Parallel Execution**: When you want to maximize development speed through parallel work
- **Resource Optimization**: When you have multiple development resources available
- **Complex Features**: When implementing complex features that can be broken into independent components

## Command Structure

### Phase 1: Plan Analysis & Dependency Mapping
**AUTONOMOUS PRINCIPAL ENGINEER DOCTRINE**: Begin with comprehensive analysis of the existing plan to understand dependencies and parallelization opportunities.

#### Plan Decomposition Analysis
- **Task Dependency Analysis**: Map all task dependencies and critical paths
- **Resource Requirement Analysis**: Identify resource requirements for each task
- **Timeline Analysis**: Understand task durations and scheduling constraints
- **Risk Assessment**: Identify risks that could affect parallel execution
- **Quality Gate Analysis**: Map quality gates and validation checkpoints

#### Dependency Graph Construction
- **Task Relationships**: Map all task-to-task dependencies
- **Resource Dependencies**: Identify shared resources and conflicts
- **Data Dependencies**: Map data flow and sharing requirements
- **Infrastructure Dependencies**: Identify shared infrastructure needs
- **External Dependencies**: Map external service and API dependencies

### Phase 2: Parallel Phase Identification
**Dependency-Free Phase Creation**: Identify groups of tasks that can be executed independently.

#### Independent Phase Detection
- **Zero-Dependency Tasks**: Identify tasks with no dependencies
- **Isolated Feature Sets**: Group related tasks that don't depend on others
- **Parallel Workstreams**: Identify workstreams that can run simultaneously
- **Resource Isolation**: Ensure phases don't compete for the same resources
- **Data Isolation**: Ensure phases don't have data conflicts

#### Phase Boundary Definition
- **Clear Boundaries**: Define clear boundaries between phases
- **Interface Definition**: Define interfaces between phases
- **Handoff Points**: Identify handoff points between phases
- **Validation Gates**: Define validation gates for each phase
- **Integration Points**: Plan integration points for phase outputs

### Phase 3: Phase Optimization & Balancing
**Optimal Resource Distribution**: Optimize phases for balanced workload and minimal dependencies.

#### Workload Balancing
- **Task Distribution**: Distribute tasks evenly across phases
- **Complexity Balancing**: Balance complexity across phases
- **Duration Balancing**: Balance estimated duration across phases
- **Resource Balancing**: Balance resource requirements across phases
- **Risk Balancing**: Distribute risks evenly across phases

#### Dependency Minimization
- **Dependency Elimination**: Remove unnecessary dependencies where possible
- **Interface Simplification**: Simplify interfaces between phases
- **Shared Resource Management**: Plan shared resource usage
- **Conflict Resolution**: Resolve resource and data conflicts
- **Timeline Optimization**: Optimize timelines for parallel execution

### Phase 4: Phase Documentation & Handoff Planning
**Comprehensive Phase Documentation**: Create detailed documentation for each parallel phase and organize them in a structured directory.

#### Phase Documentation
- **Phase Scope**: Clear definition of what each phase covers
- **Phase Objectives**: Specific objectives and success criteria
- **Phase Dependencies**: Any remaining dependencies and constraints
- **Phase Resources**: Required resources and tools
- **Phase Timeline**: Estimated duration and milestones

#### Handoff Documentation
- **Input Requirements**: What each phase needs as input
- **Output Specifications**: What each phase will produce
- **Integration Specifications**: How phase outputs will be integrated
- **Quality Gates**: Quality requirements for each phase
- **Validation Criteria**: How to validate phase completion

#### Document Organization
- **Directory Creation**: Create a directory named after the original planning document
- **Phase Document Storage**: Store each phase plan in the parallelization directory
- **Master Index**: Create a master index document listing all phases
- **Integration Guide**: Create integration guide for combining phase outputs
- **Quality Checklist**: Create quality checklist for each phase

### Phase 5: Document Creation & Organization
**Structured Document Creation**: Create and organize all parallelized documents in a structured directory.

#### Directory Structure Creation
- **Directory Naming**: Create directory named after original planning document
- **Phase Subdirectories**: Create subdirectory for each parallel phase
- **Document Templates**: Use consistent templates for all phase documents
- **File Naming**: Use consistent naming conventions for all documents
- **Path Management**: Ensure all paths are relative and portable

#### Document Creation Process
- **Master Index**: Create comprehensive README.md with project overview
- **LLM Execution Prompt**: Create LLM-EXECUTION-PROMPT.md with concise execution instructions
- **Status Tracking**: Create STATUS.md for real-time coordination between LLMs
- **Quick Start Guide**: Create QUICK-START.md for project managers/coordinators
- **Phase Documents**: Create detailed documents for each phase
- **Integration Guide**: Create integration guide for combining phases
- **Quality Checklist**: Create quality checklist for all phases
- **Handoff Specifications**: Create handoff specifications for each phase

#### Coordination Files (MANDATORY)

**LLM-EXECUTION-PROMPT.md** (MANDATORY):
- **Purpose**: Concise execution instructions for LLMs executing phases
- **Length**: Target ~100 lines (concise, not verbose)
- **Content**:
  - **Autonomous Phase Selection** (MANDATORY): Instructions for LLMs to autonomously select which phase to work on based on:
    - Availability (not in progress, dependencies met, not blocked)
    - Priority (phases that unblock others, critical path)
    - Task availability (phases with incomplete tasks)
  - Quick start steps (select phase, check STATUS.md, read phase plan, update status)
  - Real-time status update requirements (MANDATORY - update after each task)
  - Code quality principles (SOLID/DRY/YAGNI)
  - `/understand` command usage examples for existing patterns
  - Phase-specific implementation guidelines
  - Testing and documentation requirements
  - Completion checklist
  - Next phase selection after completion
- **Format**: Actionable, phase-agnostic template that works for any phase

**STATUS.md** (MANDATORY):
- **Purpose**: Real-time coordination to prevent conflicts between LLMs
- **Content**:
  - Active work tracking table: Phase | LLM | Current Task | File | Status | Started | ETA
  - Task-level status checkboxes for all phases
  - Blockers section
  - Integration points tracking
  - Instructions for LLMs to update after each task
- **Format**: Table-based, easy to update, real-time coordination
- **Update Frequency**: After EVERY task (starting, completing, blocking)

**QUICK-START.md**:
- **Purpose**: Quick reference for project managers/coordinators
- **Content**:
  - How to assign phases to LLMs
  - How to provide execution prompt to LLMs
  - How to monitor progress via STATUS.md
  - Integration steps after phases complete
  - File structure overview
- **Format**: Quick reference, not comprehensive documentation

#### Document Content Requirements
- **Consistency**: All documents follow consistent format and structure
- **Completeness**: Each document contains all required information
- **Clarity**: All documents are clear and easy to understand
- **Actionability**: All documents provide clear next steps
- **Quality Standards**: All documents meet quality standards

### Phase 6: Parallel Execution Planning
**Multi-LLM Coordination**: Plan for coordinated execution across multiple LLMs.

#### LLM Assignment Strategy
- **Phase Assignment**: Assign phases to specific LLMs
- **Resource Allocation**: Allocate resources to each LLM
- **Communication Planning**: Plan communication between LLMs
- **Progress Tracking**: Plan progress tracking across phases
- **Conflict Resolution**: Plan conflict resolution procedures

#### Coordination Mechanisms
- **Status Synchronization**: Plan status synchronization between LLMs
- **Dependency Monitoring**: Monitor dependencies between phases
- **Integration Coordination**: Coordinate integration of phase outputs
- **Quality Gate Coordination**: Coordinate quality gate validation
- **Issue Escalation**: Plan issue escalation procedures

## Parallelization Methodologies

### Dependency-Based Parallelization
- **Critical Path Analysis**: Identify critical path and parallel opportunities
- **Dependency Elimination**: Remove dependencies where possible
- **Interface Design**: Design clean interfaces between phases
- **Resource Isolation**: Ensure resource independence
- **Timeline Optimization**: Optimize for parallel execution

### Feature-Based Parallelization
- **Feature Isolation**: Isolate independent features
- **Component Separation**: Separate independent components
- **Service Decomposition**: Decompose into independent services
- **Data Partitioning**: Partition data for independent processing
- **API Separation**: Separate APIs for independent development

### Resource-Based Parallelization
- **Resource Allocation**: Allocate resources to phases
- **Resource Isolation**: Ensure resource independence
- **Resource Optimization**: Optimize resource usage
- **Resource Monitoring**: Monitor resource usage
- **Resource Scaling**: Plan resource scaling

## Phase Types & Patterns

### Independent Feature Phases
- **User Interface Phase**: Frontend components and UI
- **Backend API Phase**: Backend services and APIs
- **Database Phase**: Database schema and data management
- **Integration Phase**: External service integrations
- **Testing Phase**: Comprehensive testing and validation

### Component-Based Phases
- **Authentication Phase**: User authentication and authorization
- **Data Management Phase**: Data processing and storage
- **Business Logic Phase**: Core business logic implementation
- **Presentation Phase**: User interface and experience
- **Integration Phase**: System integration and APIs

### Service-Based Phases
- **User Service Phase**: User management and authentication
- **Product Service Phase**: Product catalog and management
- **Order Service Phase**: Order processing and management
- **Payment Service Phase**: Payment processing and billing
- **Notification Service Phase**: Notifications and communications

## Quality Standards & Requirements

### Phase Quality Standards
- **Clean Architecture**: Each phase follows clean architecture principles
- **SOLID Principles**: Each phase adheres to SOLID principles
- **Test Coverage**: Each phase includes comprehensive testing
- **Documentation**: Each phase includes complete documentation
- **Code Quality**: Each phase maintains high code quality

### Integration Quality Standards
- **Interface Compliance**: Phases comply with defined interfaces
- **Data Consistency**: Phases maintain data consistency
- **Error Handling**: Phases include proper error handling
- **Logging**: Phases include comprehensive logging
- **Monitoring**: Phases include monitoring and observability

## Parallelization Outputs

### Document Organization Structure
**Organized Directory Structure**: All parallelized documents are organized in a structured directory based on the original planning document.

#### Directory Structure
```
[original-plan-name]/
├── README.md                           # Master index and overview
├── integration-guide.md                # Integration guide for combining phases
├── quality-checklist.md                # Quality checklist for all phases
├── LLM-EXECUTION-PROMPT.md            # Execution prompt for LLMs (REQUIRED)
├── STATUS.md                           # Real-time status tracking (REQUIRED)
├── QUICK-START.md                      # Quick start guide for coordinators
├── phase-1-[phase-name]/
│   ├── phase-plan.md                   # Detailed phase plan
│   ├── requirements.md                 # Phase requirements
│   ├── architecture.md                 # Phase architecture
│   ├── testing-strategy.md             # Phase testing strategy
│   └── handoff-specification.md        # Phase handoff specification
├── phase-2-[phase-name]/
│   ├── phase-plan.md
│   ├── requirements.md
│   ├── architecture.md
│   ├── testing-strategy.md
│   └── handoff-specification.md
└── [additional-phases...]
```

#### Master Index Document (README.md)
- **Project Overview**: High-level overview of the parallelized project
- **Phase Summary**: Summary of all phases and their objectives
- **Phase Dependencies**: Visual dependency map between phases
- **Resource Allocation**: Resource allocation across phases
- **Timeline Overview**: Overall timeline and milestones
- **Quality Standards**: Quality standards applied across all phases
- **Integration Strategy**: How phases will be integrated
- **LLM Assignment**: Assignment of phases to specific LLMs
- **LLM Execution Instructions**: Copyable instructions section for LLMs executing phases (see "LLM Execution Instructions" section below)

### Phase Plans
- **Individual Phase Plans**: Detailed plans for each parallel phase
- **Phase Dependencies**: Clear dependency mapping
- **Phase Interfaces**: Interface specifications between phases
- **Phase Timelines**: Detailed timelines for each phase
- **Phase Resources**: Resource requirements for each phase

### Coordination Plans
- **LLM Assignment Plan**: Assignment of phases to LLMs
- **Communication Plan**: Communication protocols between LLMs
- **Progress Tracking Plan**: Progress tracking and reporting
- **Integration Plan**: Integration strategy for phase outputs
- **Quality Assurance Plan**: Quality assurance across phases

### Handoff Artifacts
- **Phase Handoff Documents**: Handoff documentation for each phase
- **Integration Specifications**: Integration specifications
- **Quality Gate Definitions**: Quality gate definitions
- **Validation Procedures**: Validation procedures
- **Issue Resolution Procedures**: Issue resolution procedures

## Usage Examples

### Large-Scale Project Parallelization
```
/parallelize
Break down this comprehensive e-commerce platform plan into parallel phases that can be executed by multiple LLMs simultaneously.

Output: Creates e-commerce-platform/ directory with:
├── README.md
├── integration-guide.md
├── quality-checklist.md
├── phase-1-frontend/
├── phase-2-backend/
├── phase-3-database/
└── phase-4-integration/
```

### Feature-Based Parallelization
```
/parallelize
Parallelize this user authentication feature plan into independent phases for frontend, backend, database, and testing.

Output: Creates user-authentication/ directory with:
├── README.md
├── integration-guide.md
├── quality-checklist.md
├── phase-1-frontend-auth/
├── phase-2-backend-auth/
├── phase-3-database-auth/
└── phase-4-testing-auth/
```

### Service-Based Parallelization
```
/parallelize
Break down this microservices architecture plan into parallel phases for each service that can be developed independently.

Output: Creates microservices-architecture/ directory with:
├── README.md
├── integration-guide.md
├── quality-checklist.md
├── phase-1-user-service/
├── phase-2-product-service/
├── phase-3-order-service/
└── phase-4-payment-service/
```

### Component-Based Parallelization
```
/parallelize
Parallelize this React application plan into phases for components, services, utilities, and testing that can be developed in parallel.

Output: Creates react-application/ directory with:
├── README.md
├── integration-guide.md
├── quality-checklist.md
├── phase-1-components/
├── phase-2-services/
├── phase-3-utilities/
└── phase-4-testing/
```

### Multi-Team Parallelization
```
/parallelize
Create parallel phases for this project that can be assigned to different development teams working simultaneously.

Output: Creates [project-name]/ directory with organized phase subdirectories
```

## Document Creation Requirements

### MANDATORY Document Organization
**Every parallelization MUST create a structured directory with comprehensive documentation.**

#### Directory Structure Requirements
- **Directory Naming**: Directory named after original planning document
- **Phase Subdirectories**: Each phase gets its own subdirectory
- **Consistent Naming**: All files follow consistent naming conventions
- **Relative Paths**: All paths are relative and portable
- **Template Consistency**: All documents follow consistent templates

#### Required Documents
- **Master README.md**: Comprehensive project overview and phase index
- **Integration Guide**: Detailed guide for integrating phase outputs
- **Quality Checklist**: Quality checklist for all phases
- **LLM-EXECUTION-PROMPT.md**: Concise execution prompt for LLMs (MANDATORY)
- **STATUS.md**: Real-time status tracking for coordination (MANDATORY)
- **QUICK-START.md**: Quick start guide for project managers/coordinators
- **Phase Plans**: Detailed plan for each parallel phase
- **Handoff Specifications**: Handoff specifications for each phase

#### Document Content Standards
- **Completeness**: Each document contains all required information
- **Clarity**: All documents are clear and easy to understand
- **Actionability**: All documents provide clear next steps
- **Consistency**: All documents follow consistent format
- **Quality Standards**: All documents meet quality standards

## Success Criteria

### Parallelization Quality
- **Independence**: Phases are truly independent with no dependencies
- **Balance**: Phases are well-balanced in terms of workload and complexity
- **Clarity**: Phase boundaries and interfaces are clearly defined
- **Feasibility**: Phases are realistic and achievable
- **Integration**: Phase integration is well-planned and feasible

### Document Organization Quality
- **Structure**: Documents are well-organized in clear directory structure
- **Completeness**: All required documents are created
- **Consistency**: All documents follow consistent format and style
- **Accessibility**: Documents are easy to find and navigate
- **Maintainability**: Documents are easy to maintain and update

### Execution Efficiency
- **Parallel Execution**: Phases can be executed in parallel
- **Resource Utilization**: Resources are optimally utilized
- **Timeline Optimization**: Overall timeline is optimized
- **Quality Maintenance**: Quality is maintained across phases
- **Risk Mitigation**: Risks are minimized and managed

## Coordination Files Templates

### LLM-EXECUTION-PROMPT.md Template Structure

When creating LLM-EXECUTION-PROMPT.md, include:
1. **Autonomous Phase Selection** (MANDATORY): 
   - Instructions for analyzing available phases
   - Priority order (critical path, foundation, deployment)
   - Selection criteria (availability, dependencies, blockers)
   - How to claim phase in STATUS.md
   - Next phase selection after completion
2. **Quick Start**: Select phase, check STATUS.md, read phase plan, update status
3. **Real-Time Status Updates**: MANDATORY update requirements
4. **Understand Patterns**: `/understand` command usage examples
5. **Code Quality**: SOLID/DRY/YAGNI principles
6. **Phase Guidelines**: Phase-specific implementation notes
7. **Testing & Docs**: Requirements for tests and documentation
8. **Completion Checklist**: What to verify before marking complete

### STATUS.md Template Structure

When creating STATUS.md, include:
1. **Active Work Table**: Track current work across all phases
2. **Task Status**: Checkboxes for all tasks in all phases
3. **Blockers Section**: Document any blockers
4. **Integration Points**: Track when integration points are ready
5. **Update Instructions**: Clear instructions for LLMs

### QUICK-START.md Template Structure

When creating QUICK-START.md, include:
1. **Autonomous Phase Selection**: LLMs autonomously select phases (no manual assignment needed)
2. **Execution Setup**: How to provide LLM-EXECUTION-PROMPT.md to LLMs
3. **Progress Monitoring**: How to track via STATUS.md
4. **Phase Priority**: Overview of critical path and priority order
5. **Integration Steps**: Steps after phases complete
6. **File Structure**: Overview of created files

## LLM Execution Instructions

### Copyable Instructions for Phase Execution

When a plan has been parallelized, use the following instructions for LLMs executing individual phases:

**Note**: The LLM-EXECUTION-PROMPT.md file contains the actual execution prompt. Use that file when assigning work to LLMs.

```
# Phase Execution Instructions

## Step 1: Read the Master Plan
1. Navigate to the parallelized plan directory: `[plan-directory]/`
2. Read `README.md` to understand:
   - Overall project overview and objectives
   - All phases and their relationships
   - Phase dependencies (if any)
   - Resource allocation and timeline
   - Quality standards and integration strategy

## Step 2: Autonomously Select Your Phase
1. **Analyze Available Phases**:
   - Read `STATUS.md` to check which phases are in progress or blocked
   - Review phase dependencies in `README.md` dependency map
   - Check task-level status to see which phases have incomplete tasks
2. **Apply Selection Criteria**:
   - Prioritize phases that unblock others (critical path)
   - Select phases with dependencies met (prerequisite phases complete)
   - Choose phases not currently in progress
   - Prefer phases with most incomplete tasks if multiple available
3. **Claim Phase in STATUS.md**:
   - Update "Active Work" table with your selection
   - Mark phase as 🔄 In Progress with your identifier
   - Add timestamp and ETA
4. Navigate to your phase directory: `[plan-directory]/phase-[number]-[phase-name]/`

## Step 3: Read Your Phase Documentation
Read these documents in order:
1. `phase-plan.md` - Complete phase plan with objectives and scope
2. `requirements.md` - Detailed requirements and acceptance criteria
3. `architecture.md` - Technical architecture and design decisions
4. `testing-strategy.md` - Testing approach and validation requirements
5. `handoff-specification.md` - Input requirements and output specifications

## Step 4: Verify Phase Independence
Before starting execution, verify:
- [ ] No dependencies on other phases (check `README.md` dependency map)
- [ ] All required inputs are available (check `handoff-specification.md`)
- [ ] Phase interfaces are clearly defined
- [ ] Quality gates are understood

## Step 5: Execute Your Phase
1. Follow the implementation plan in `phase-plan.md`
2. Adhere to architecture specifications in `architecture.md`
3. Implement according to requirements in `requirements.md`
4. Follow testing strategy in `testing-strategy.md`
5. Ensure all outputs match `handoff-specification.md`

## Step 6: Validate Phase Completion
Before marking phase complete, verify:
- [ ] All requirements from `requirements.md` are met
- [ ] All tests from `testing-strategy.md` pass
- [ ] Outputs match `handoff-specification.md` format
- [ ] Code quality standards are met (check `quality-checklist.md`)
- [ ] Documentation is complete and up-to-date

## Step 7: Document Phase Completion
1. Update phase status in `README.md` (mark phase as complete)
2. Document any deviations from plan in phase directory
3. Create handoff artifacts as specified in `handoff-specification.md`
4. Update integration guide if phase outputs affect integration

## Step 8: Coordinate with Other Phases
1. Check `README.md` for dependent phases that may be waiting
2. Communicate phase completion to dependent phases if needed
3. Update shared status tracking if available
4. Review integration guide for any integration requirements

## Execution Order Guidelines

### If Phases Have Dependencies:
- Execute phases in dependency order (phases with no dependencies first)
- Wait for prerequisite phases to complete before starting dependent phases
- Verify prerequisite phase outputs before beginning execution

### If Phases Are Independent:
- Phases can be executed in parallel by different LLMs
- No coordination needed during execution
- Only coordinate during integration phase

### Integration Phase:
- Execute after all prerequisite phases are complete
- Follow `integration-guide.md` for combining phase outputs
- Validate integrated system against overall project requirements

## Quality Standards
- Follow all quality standards in `quality-checklist.md`
- Maintain code quality and testing standards
- Ensure proper error handling and logging
- Document all decisions and deviations
- Follow project coding standards and conventions

## Issue Resolution
- If blocked, document issue in phase directory
- Check `README.md` for common issues and solutions
- Escalate to project coordinator if needed
- Update phase status to "blocked" if unable to proceed
```

## Integration with Other Commands

### Pre-Parallelization
- Use with `/plan` to create the initial comprehensive plan
- Use with `/understand` to understand project requirements
- Use with `/analyze-project-deep` to understand project complexity

### Post-Parallelization
- Use with `/implement` to execute individual phases
- Use with `/status` to track progress across phases
- Use with `/test` to validate phase outputs
- Use with `/debug` to resolve phase issues

### Coordination
- Use with `/status` to coordinate between LLMs
- Use with `/retro` to learn from parallel execution
- Use with `/plan` to adjust plans based on parallel execution results

**Remember**: The `/parallelize` command is your multi-agent coordination engine. Use it to break down large plans into parallel-executable phases that can be tackled by multiple LLMs simultaneously, maximizing development speed while maintaining quality and coordination.
