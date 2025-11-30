# Implement Command

## Purpose
Execute planned development work using Cursor's recommended structured approach: modular execution, iterative validation, and comprehensive quality assurance. This command follows the "edit-test-fix" loop with automated testing and error correction.

## When to Use
- **Feature Implementation**: When implementing planned features or functionality
- **Bug Fixes**: When implementing planned bug fixes or system improvements
- **Refactoring**: When implementing planned code refactoring or improvements
- **Architecture Changes**: When implementing planned architectural modifications
- **Integration Work**: When implementing planned integrations or API changes
- **Performance Improvements**: When implementing planned performance optimizations

## Command Structure

### Phase 1: Pre-Implementation Validation & Status Assessment
**AUTONOMOUS PRINCIPAL ENGINEER DOCTRINE**: Ensure all prerequisites are met and assess current implementation status before beginning work.

#### Plan Discovery & Reading
- **Plan Location Search**: Search for planning documents in `planning/[ticket-id]/` directory
- **Plan Reading**: Read and understand the planning document if it exists
- **Plan Update Instructions**: Identify and understand any plan update requirements in the plan
- **Task Extraction**: Extract all executable tasks from the planning document
- **Plan Validation**: Verify plan structure and completeness

#### Plan Creation (If Needed)
- **No Plan Detected**: If no planning document exists, use `/save-plan` command to create one
- **No Tasks in Plan**: If plan exists but has no executable tasks, use `/save-plan` to create/update tasks
- **Plan Validation**: Ensure created/updated plan has clear, actionable tasks before proceeding
- **Plan Structure**: Verify plan follows standard structure with progress tracking, task breakdown, and next steps

#### Status Assessment
- **Previous Session Review**: Review work completed in previous LLM sessions via planning document
- **Current State Analysis**: Analyze current implementation status and progress from plan
- **Context Recovery**: Recover context and decision rationale from planning document
- **Blocker Identification**: Identify any blockers or unresolved issues from plan
- **Next Steps Validation**: Validate planned next steps from plan against current state

#### Plan Validation
- **Requirements Verification**: Confirm all requirements are clear and complete in plan
- **Technical Feasibility**: Verify technical approach is sound and achievable per plan
- **Dependency Check**: Ensure all dependencies and prerequisites are available per plan
- **Resource Validation**: Confirm required resources and tools are available per plan
- **Risk Assessment**: Review identified risks and mitigation strategies from plan

#### Environment Preparation
- **Development Environment**: Ensure development environment is properly configured
- **Testing Environment**: Verify testing environment is ready for validation
- **Version Control**: Confirm proper branching and version control setup
- **Documentation**: Ensure relevant documentation is accessible
- **Team Coordination**: Verify team members are aligned and available

### Phase 2: Modular Implementation Execution
**Incremental Development Approach**: Implement work in small, well-isolated modules following Cursor's recommendations.

#### Module-Based Implementation
- **Single Responsibility**: Each module has one clear, focused responsibility
- **Well-Isolated**: Modules are independent and can be developed separately
- **Testable**: Each module can be tested in isolation
- **Incremental**: Modules build upon each other in logical sequence
- **Validated**: Each module is validated before proceeding to the next

#### Implementation Strategy
- **Start Small**: Begin with the simplest, most foundational modules
- **Build Up**: Gradually add complexity and functionality
- **Test Continuously**: Test each module as it's implemented
- **Fix Immediately**: Address issues as they're discovered
- **Document Progress**: Maintain clear documentation of implementation progress

#### 🚨 MANDATORY Plan Updates After Each Task
**CRITICAL**: After completing ANY task, you MUST update the planning document IMMEDIATELY.

**After completing each task:**
1. **Update Planning Document**: Run `/save-plan` to update the planning document with task completion
2. **Mark Task Complete**: Update task status to ✅ Complete in the plan
3. **Update Progress Metrics**: Update task counters, completion percentages, and phase status
4. **Update "What We've Done"**: Add completed task details to the "What We've Done" section
5. **Update "Currently Working On"**: Set to the next task or mark as complete
6. **Update Files Modified**: Document any files created, modified, or deleted
7. **Update "Last Updated"**: Refresh timestamp to current date/time
8. **Update Next Steps**: Ensure next steps are clear and actionable

**This is MANDATORY - even if the plan doesn't explicitly state update requirements. Plan updates are required for multi-session continuity.**

**Plan Update Protocol:**
- **Read Plan Update Instructions**: Check if plan has specific update instructions and follow them
- **Standard Update Process**: Even if plan lacks explicit instructions, follow standard update protocol above
- **Immediate Updates**: Update plan immediately after task completion, before starting next task
- **Use `/save-plan` Command**: Always use the `/save-plan` command to persist plan updates

### Phase 3: Automated Testing & Validation
**Edit-Test-Fix Loop**: Implement Cursor's recommended automated testing and error correction workflow.

#### Continuous Testing
- **Unit Testing**: Automated unit tests for each module
- **Integration Testing**: Automated integration tests for module interactions
- **End-to-End Testing**: Automated E2E tests for complete workflows
- **Performance Testing**: Automated performance tests for critical paths
- **Security Testing**: Automated security tests for vulnerabilities

#### Automated Error Correction
- **Error Detection**: Automated detection of implementation errors
- **Error Analysis**: Automated analysis of error causes and impacts
- **Error Correction**: Automated correction of identified issues
- **Validation**: Automated validation of corrections
- **Regression Testing**: Automated testing to prevent regressions

### Phase 4: Quality Assurance & Code Review
**Multi-Layer Verification**: Ensure code quality and adherence to standards.

#### Code Quality Checks
- **Linting**: Automated code style and quality checks
- **Type Checking**: Automated type safety validation
- **Security Scanning**: Automated security vulnerability scanning
- **Performance Analysis**: Automated performance impact analysis
- **Dependency Analysis**: Automated dependency and vulnerability scanning

#### Code Review Process
- **Automated Review**: Automated code review using AI and static analysis
- **Peer Review**: Human code review for complex or critical changes
- **Architecture Review**: Review of architectural decisions and patterns
- **Security Review**: Security-focused review of sensitive code
- **Performance Review**: Performance-focused review of critical paths

### Phase 5: Integration & System Testing
**System-Wide Validation**: Ensure implementation works correctly within the broader system.

#### Integration Testing
- **Module Integration**: Test integration between implemented modules
- **System Integration**: Test integration with existing system components
- **API Integration**: Test integration with external APIs and services
- **Database Integration**: Test integration with data storage systems
- **User Interface Integration**: Test integration with user interface components

#### System Validation
- **Functional Testing**: Verify all functional requirements are met
- **Performance Testing**: Verify performance requirements are satisfied
- **Security Testing**: Verify security requirements are implemented
- **Usability Testing**: Verify user experience requirements are met
- **Compatibility Testing**: Verify compatibility with target environments

### Phase 6: Status Tracking & Session Handoff
**Multi-Session Continuity**: Track implementation status and prepare for seamless handoff to subsequent LLM sessions.

#### Status Tracking
- **Implementation Log**: Document all work completed in this session
- **Progress Update**: Update progress status of all tasks and milestones
- **Quality Status**: Update status of all quality gates and validation checkpoints
- **Issue Resolution**: Document all issues resolved and blockers encountered
- **Context Preservation**: Preserve current context and decision rationale

#### Session Handoff Preparation
- **Next Steps Definition**: Clearly define next steps for subsequent sessions
- **Blocker Documentation**: Document any unresolved blockers or issues
- **Context Summary**: Create comprehensive context summary for next session
- **Knowledge Transfer**: Document session-specific knowledge and learnings
- **Handoff Artifacts**: Create handoff artifacts for seamless continuation

#### Handoff Artifacts
- **Implementation Status Report**: Current status of all implementation tasks
- **Progress Summary**: Summary of progress made in this session
- **Quality Gate Status**: Status of all quality gates and validation checkpoints
- **Issue Log**: Log of all issues, resolutions, and remaining blockers
- **Context Documentation**: Complete context and decision rationale
- **Next Session Brief**: Clear brief for the next LLM session

### Phase 7: Documentation & Knowledge Transfer
**Comprehensive Documentation**: Ensure implementation is properly documented and knowledge is transferred.

#### Implementation Documentation
- **Code Documentation**: Comprehensive code comments and documentation
- **API Documentation**: Complete API documentation and examples
- **Architecture Documentation**: Updated architecture documentation
- **Deployment Documentation**: Deployment and configuration documentation
- **User Documentation**: User guides and help documentation

#### Knowledge Transfer
- **Team Knowledge**: Transfer knowledge to team members
- **Stakeholder Communication**: Communicate implementation results to stakeholders
- **Training Materials**: Create training materials for users and maintainers
- **Best Practices**: Document lessons learned and best practices
- **Future Maintenance**: Document maintenance and support procedures

## Implementation Methodologies

### Agile Implementation
- **Sprint-Based**: Implement work in short sprints with regular validation
- **User Story Driven**: Focus on delivering user value incrementally
- **Continuous Integration**: Integrate changes continuously with automated testing
- **Retrospective Learning**: Learn from each implementation cycle
- **Adaptive Planning**: Adjust implementation based on feedback and learning

### Waterfall Implementation
- **Phase-Based**: Implement work in distinct phases with clear deliverables
- **Documentation-Heavy**: Emphasize comprehensive documentation
- **Quality Gates**: Strict quality criteria for each phase
- **Change Control**: Formal change management processes
- **Final Integration**: Comprehensive integration at the end

### Hybrid Implementation
- **Adaptive Approach**: Combine methodologies based on project needs
- **Risk-Based**: Adjust approach based on risk assessment
- **Stakeholder-Driven**: Adapt to stakeholder preferences and needs
- **Technology-Adaptive**: Adjust methodology based on technology stack
- **Context-Sensitive**: Adapt implementation approach to project context

## Technology-Specific Implementation

### Frontend Implementation
- **Component Architecture**: Implement using appropriate component patterns
- **State Management**: Implement state management following best practices
- **Performance Optimization**: Implement performance optimizations
- **Accessibility**: Implement accessibility features and compliance
- **Responsive Design**: Implement responsive design patterns

### Backend Implementation
- **API Design**: Implement RESTful or GraphQL APIs following best practices
- **Database Design**: Implement database schemas and access patterns
- **Security Implementation**: Implement authentication, authorization, and security
- **Performance Optimization**: Implement caching, optimization, and scaling
- **Monitoring**: Implement logging, monitoring, and observability

### Full-Stack Implementation
- **End-to-End Integration**: Implement complete user workflows
- **Data Flow**: Implement data flow between frontend and backend
- **Real-Time Features**: Implement real-time communication and updates
- **Cross-Platform**: Implement for multiple platforms and devices
- **Scalability**: Implement for scalability and high availability

## Implementation Outputs

### Code Deliverables
- **Source Code**: Complete, tested, and documented source code
- **Tests**: Comprehensive test suite with good coverage
- **Documentation**: Complete technical and user documentation
- **Configuration**: Environment and deployment configuration
- **Scripts**: Build, test, and deployment scripts

### Quality Metrics
- **Code Coverage**: Test coverage metrics and reports
- **Performance Metrics**: Performance benchmarks and monitoring
- **Security Metrics**: Security scan results and compliance
- **Quality Metrics**: Code quality scores and analysis
- **User Metrics**: User experience and satisfaction metrics

### Knowledge Artifacts
- **Architecture Decisions**: Documented architectural decisions and rationale
- **Lessons Learned**: Documented lessons learned and best practices
- **Training Materials**: Training materials and knowledge transfer documents
- **Maintenance Guides**: Maintenance and support documentation
- **Future Roadmap**: Recommendations for future improvements and enhancements

## Status Tracking Requirements

### MANDATORY STATUS TRACKING
**Every implementation session MUST maintain comprehensive status tracking for multi-session continuity.**

#### Required Status Artifacts
- **Implementation Status Report**: Current status of all tasks and milestones
- **Progress Summary**: What was completed in this session
- **Quality Gate Status**: Status of all validation checkpoints
- **Issue Log**: All issues encountered and their resolution status
- **Context Documentation**: Complete context and decision rationale
- **Next Session Brief**: Clear instructions for the next LLM session

#### Status Tracking Format
```markdown
## Implementation Status Report
**Session Date**: [Date]
**Session Duration**: [Duration]
**LLM Session ID**: [Session Identifier]

### Completed Tasks
- [ ] Task 1: [Description] - [Status: Completed/In Progress/Blocked]
- [ ] Task 2: [Description] - [Status: Completed/In Progress/Blocked]

### Progress Summary
- **Tasks Completed**: [Number] of [Total]
- **Quality Gates Passed**: [Number] of [Total]
- **Issues Resolved**: [Number]
- **Blockers Identified**: [Number]

### Quality Gate Status
- [ ] Unit Testing: [Status]
- [ ] Integration Testing: [Status]
- [ ] Code Review: [Status]
- [ ] Performance Validation: [Status]
- [ ] Security Validation: [Status]

### Issues and Blockers
- **Resolved Issues**: [List of resolved issues]
- **Active Blockers**: [List of current blockers]
- **Next Steps**: [Clear next steps for next session]

### Context for Next Session
- **Current State**: [Description of current implementation state]
- **Key Decisions**: [Important decisions made in this session]
- **Dependencies**: [Current dependencies and their status]
- **Environment**: [Current environment and configuration state]
```

## Usage Examples

### Feature Implementation
```
/implement
Implement the user authentication feature as planned, including OAuth2 integration, user management, and security features.
```

### Bug Fix Implementation
```
/implement
Implement the fix for the memory leak issue, including root cause resolution, performance improvements, and prevention measures.
```

### Refactoring Implementation
```
/implement
Implement the planned refactoring of the user service, including improved error handling, performance optimization, and code organization.
```

### Architecture Implementation
```
/implement
Implement the microservices architecture as planned, including service decomposition, API design, and deployment configuration.
```

### Multi-Session Continuation
```
/implement
Continue implementation from previous session. Review status report and continue with next planned tasks.
```

## Success Criteria

### Implementation Quality
- **Functional Completeness**: All planned functionality is implemented
- **Code Quality**: High-quality, maintainable, and well-documented code
- **Test Coverage**: Comprehensive test coverage with automated testing
- **Performance**: Meets or exceeds performance requirements
- **Security**: Implements security best practices and requirements

### Delivery Success
- **On Time**: Delivered within planned timeline
- **On Budget**: Delivered within planned resource allocation
- **On Scope**: Delivered according to planned scope and requirements
- **Quality Standards**: Meets or exceeds quality standards
- **Stakeholder Satisfaction**: Meets stakeholder expectations and requirements

## Integration with Other Commands

### Pre-Implementation
- Use with `/save-plan` to create or update planning documents before implementation
- Use with `/plan` to ensure comprehensive planning before implementation
- Use with `/understand` to build understanding before implementation
- Use with `/analyze-code-deep` to understand existing code before changes

### During Implementation
- **ALWAYS use `/save-plan`** after completing each task to update planning document
- **MANDATORY**: Update plan after every task completion, regardless of plan instructions
- Use with `/test` to implement comprehensive testing strategies
- Use with `/debug` to troubleshoot implementation issues
- Use with `/refactor` to improve code during implementation

### Post-Implementation
- Use with `/analyze-performance` to validate performance improvements
- Use with `/analyze-accessibility` to validate accessibility compliance
- Use with `/retro` to learn from implementation experience

**Remember**: The `/implement` command is your execution engine for turning plans into reality. Use it to implement work using Cursor's recommended structured approach with modular execution, iterative validation, and comprehensive quality assurance. The command adapts to your specific needs and provides the level of implementation detail appropriate for your context.
