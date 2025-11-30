# Feature Implementation Protocol

## Overview

Execute systematic feature implementation based on existing design specification. This command reads design context from prompt history or saved context files and implements the feature following the approved design.

## Mission Briefing: Feature Implementation Protocol

**ANALYSIS COMMAND DETECTION:** Before proceeding with implementation, check if the provided text contains any `/analyze*` command (e.g., `/analyze-general`, `/analyze-code`, `/analyze-project`, `/analyze-bug-*`, etc.). If found, skip this command's implementation and execute the detected command instead to avoid duplicate analysis.

You will now execute systematic feature implementation using the **AUTONOMOUS PRINCIPAL ENGINEER - OPERATIONAL DOCTRINE.** This implementation follows the approved design specification with systematic execution and quality assurance. The goal is to implement the feature exactly as designed with proper testing and validation.

---

## **Phase 0: Design Context Retrieval (Read-Only)**

**Directive:** Retrieve and validate the design specification from available sources.

**Context Sources (in priority order):**

1. **Prompt History Analysis**: Search recent conversation history for design specifications
2. **Context Files**: Look for saved design context files (e.g., `.cursor/context/feature-design-*.json`)
3. **Design References**: Check for explicit design document references in the prompt
4. **User Specification**: Parse any design details provided directly in the prompt

**Design Validation:**

- **Completeness Check**: Ensure all required design elements are present
- **Consistency Validation**: Verify design elements are internally consistent
- **Implementation Readiness**: Confirm design is detailed enough for implementation
- **Dependency Verification**: Validate all dependencies and requirements are specified

**Required Design Elements:**

- **Feature Requirements**: Complete functional and technical requirements
- **Architecture Design**: Component structure and relationships
- **Implementation Plan**: Step-by-step implementation guide
- **File Structure**: Target files and new files to create
- **Code Patterns**: Conventions and patterns to follow
- **Testing Strategy**: Testing requirements and scenarios
- **Integration Points**: Dependencies and integration requirements

**Constraints:**

- **No implementation without complete design**: If design is incomplete, request clarification
- **No design modifications**: Implement exactly as specified in the design
- **No scope creep**: Stick strictly to the approved design specification

---

## **Phase 1: Implementation Preparation**

**Directive:** Prepare the implementation environment and validate prerequisites.

**Preparation Steps:**

1. **Design Specification Review**:
   - Review complete design specification
   - Understand all requirements and constraints
   - Identify implementation phases and dependencies
   - Plan implementation sequence

2. **Environment Validation**:
   - Verify target files exist and are accessible
   - Check required dependencies are available
   - Validate build system and configuration
   - Confirm development environment is ready

3. **Implementation Planning**:
   - Break down implementation into logical phases
   - Identify critical path and dependencies
   - Plan testing and validation steps
   - Prepare rollback strategy if needed

4. **Quality Gates Setup**:
   - Set up testing framework and tools
   - Configure linting and code quality checks
   - Prepare performance monitoring
   - Set up integration testing environment

**Output:** Validated implementation plan with quality gates and rollback strategy.

---

## **Phase 2: Systematic Implementation**

**Directive:** Implement the feature following the approved design specification exactly.

**Implementation Protocol:**

1. **Follow Design Specification**: Implement exactly as designed and approved
2. **Systematic Implementation**: Follow the implementation plan step by step
3. **Quality Assurance**: Implement testing and validation as designed
4. **Documentation**: Update documentation as specified in the design
5. **Integration**: Ensure proper integration with existing system

**Implementation Phases:**

### **Phase 2.1: Core Implementation**
- **File Creation**: Create new files as specified in design
- **Component Implementation**: Implement components following design patterns
- **Service Implementation**: Create services and business logic
- **Data Layer**: Implement data models and persistence
- **API Integration**: Implement external service integrations

### **Phase 2.2: Integration Implementation**
- **Dependency Integration**: Integrate with existing dependencies
- **State Management**: Implement state management patterns
- **Routing Integration**: Add routing and navigation
- **Authentication**: Implement security and access control
- **Error Handling**: Add comprehensive error handling

### **Phase 2.3: UI/UX Implementation**
- **Component Styling**: Implement styling and theming
- **Responsive Design**: Add responsive design patterns
- **Accessibility**: Implement accessibility features
- **User Interactions**: Add user interaction patterns
- **Performance Optimization**: Implement performance optimizations

### **Phase 2.4: Testing Implementation**
- **Unit Tests**: Implement unit tests for all components
- **Integration Tests**: Add integration tests for workflows
- **E2E Tests**: Create end-to-end test scenarios
- **Performance Tests**: Add performance testing
- **Accessibility Tests**: Implement accessibility testing

**Implementation Standards:**

- **Code Quality**: Follow established coding standards and patterns
- **Type Safety**: Maintain strict TypeScript typing
- **Error Handling**: Implement comprehensive error handling
- **Performance**: Optimize for performance and scalability
- **Accessibility**: Ensure accessibility compliance
- **Testing**: Maintain high test coverage and quality

---

## **Phase 3: Quality Assurance & Validation**

**Directive:** Validate implementation against design specification and quality standards.

**Validation Process:**

1. **Design Compliance Check**:
   - Verify implementation matches design specification
   - Check all requirements are implemented
   - Validate architecture and patterns are followed
   - Confirm integration points work correctly

2. **Quality Assurance Testing**:
   - Run comprehensive test suite
   - Perform integration testing
   - Execute end-to-end testing
   - Validate performance benchmarks

3. **Code Quality Validation**:
   - Run linting and code quality checks
   - Verify TypeScript compilation
   - Check for security vulnerabilities
   - Validate accessibility compliance

4. **User Experience Validation**:
   - Test user workflows and interactions
   - Validate responsive design
   - Check accessibility features
   - Verify performance in real conditions

**Quality Gates:**

- **All Tests Pass**: Unit, integration, and E2E tests must pass
- **Code Quality**: No linting errors or quality issues
- **Performance**: Meets performance requirements
- **Accessibility**: Passes accessibility validation
- **Security**: No security vulnerabilities
- **Design Compliance**: Matches design specification exactly

---

## **Phase 4: Documentation & Handover**

**Directive:** Complete documentation and prepare for handover.

**Documentation Process:**

1. **Code Documentation**:
   - Update inline code documentation
   - Add JSDoc comments for public APIs
   - Document complex business logic
   - Update README files as needed

2. **Implementation Documentation**:
   - Document implementation decisions
   - Record any deviations from design
   - Update architecture documentation
   - Create maintenance guidelines

3. **User Documentation**:
   - Update user guides and help documentation
   - Create feature usage examples
   - Document configuration options
   - Update API documentation

4. **Testing Documentation**:
   - Document test scenarios and coverage
   - Create testing guidelines
   - Update CI/CD documentation
   - Document performance benchmarks

**Handover Preparation:**

- **Implementation Summary**: Complete summary of what was implemented
- **Testing Results**: Comprehensive testing results and coverage
- **Performance Metrics**: Performance benchmarks and optimization results
- **Known Issues**: Any known issues or limitations
- **Maintenance Notes**: Important maintenance and support information

---

## **Phase 5: Final Validation & Sign-off**

**Directive:** Perform final validation and prepare for production deployment.

**Final Validation:**

1. **Complete System Testing**:
   - Full system integration testing
   - Performance testing under load
   - Security testing and validation
   - Accessibility compliance testing

2. **Production Readiness Check**:
   - Verify all dependencies are production-ready
   - Check configuration and environment setup
   - Validate deployment procedures
   - Confirm monitoring and logging

3. **User Acceptance Validation**:
   - Validate feature meets user requirements
   - Confirm user workflows work correctly
   - Check responsive design and accessibility
   - Verify performance meets expectations

**Sign-off Criteria:**

- **Design Compliance**: Implementation matches design specification
- **Quality Standards**: Meets all quality and performance requirements
- **Testing Complete**: All tests pass and coverage is adequate
- **Documentation Complete**: All documentation is updated and complete
- **Production Ready**: Feature is ready for production deployment

---

## **Usage Examples**

- `/implement-feature` (reads design from prompt history)
- `/implement-feature using design from @.cursor/context/feature-design-user-profile.json`
- `/implement-feature based on the design we just created`
- `/implement-feature following the dashboard widget design specification`

## **Implementation Features**

- **Design-Driven**: Implements exactly as specified in design
- **Systematic Execution**: Follows structured implementation process
- **Quality Focus**: Comprehensive testing and validation
- **Documentation**: Complete documentation and handover
- **Production Ready**: Ensures production readiness
- **Rollback Safe**: Includes rollback strategy and validation

**Remember**: This command implements features based on existing design specifications. The design must be complete and approved before implementation begins.
