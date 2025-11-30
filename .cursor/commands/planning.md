# Planning Command

## Purpose
Comprehensive feature planning, bug-fix analysis, and technical specification development with persistent documentation. This command ensures thorough planning before any implementation begins.

**Output Location**: All planning documents are saved in the `planning/` folder for organized tracking and easy access.

## When to Use
- **Feature Development**: Planning new features, enhancements, or user stories
- **Bug Fixes**: Analyzing and planning solutions for reported issues
- **Technical Debt**: Planning refactoring, optimization, or architectural improvements
- **Integration Work**: Planning API integrations, third-party service connections
- **Migration Projects**: Planning framework upgrades, data migrations, or system changes
- **Performance Optimization**: Planning performance improvements and monitoring

## Command Structure

### Phase 1: Planning Initialization & Document Management
**AUTONOMOUS PRINCIPAL ENGINEER DOCTRINE**: Begin with comprehensive planning setup and document management to ensure persistent, detailed planning documentation.

#### Planning Document Management
- **Document Identification**: Determine if working on existing planning document or creating new one
- **Document Location**: Use `planning/` directory for planning documents
- **Folder Creation**: Ensure `planning/` directory exists before creating documents
- **Document Naming**: Use descriptive names like `feature-name-planning.md` or `bug-fix-issue-planning.md`
- **Document Structure**: Follow consistent planning document template
- **Version Control**: Track planning iterations and updates

#### Planning Scope Assessment
- **Feature Scope**: Define what needs to be built or fixed
- **Technical Scope**: Identify technical components and dependencies
- **User Impact**: Assess user experience and business impact
- **Timeline Scope**: Estimate effort and identify critical path
- **Risk Scope**: Identify potential risks and mitigation strategies

### Phase 2: Comprehensive Planning Execution
**Technology-Adaptive Protocols**: Apply planning methods appropriate to the type of work being planned.

#### Feature Planning
- **Requirements Analysis**: Extract and document functional and non-functional requirements
- **User Story Mapping**: Create user stories and acceptance criteria
- **Technical Architecture**: Design system architecture and component relationships
- **API Design**: Plan API endpoints, data structures, and integration points
- **Database Design**: Plan data models, relationships, and migrations
- **UI/UX Design**: Plan user interface components and user experience flows
- **Testing Strategy**: Plan unit, integration, and E2E testing approaches
- **Deployment Strategy**: Plan deployment, monitoring, and rollback procedures

#### Bug Fix Planning
- **Issue Analysis**: Understand root cause and impact of the bug
- **Reproduction Steps**: Document steps to reproduce the issue
- **Root Cause Investigation**: Identify underlying technical causes
- **Solution Design**: Plan technical solution and implementation approach
- **Testing Strategy**: Plan testing to verify fix and prevent regression
- **Risk Assessment**: Identify risks of the fix and mitigation strategies
- **Rollback Plan**: Plan rollback strategy if fix causes issues

#### Technical Debt Planning
- **Debt Assessment**: Identify and categorize technical debt items
- **Impact Analysis**: Assess impact of technical debt on system
- **Refactoring Strategy**: Plan systematic refactoring approach
- **Migration Planning**: Plan framework upgrades or architectural changes
- **Performance Planning**: Plan performance optimization strategies
- **Monitoring Strategy**: Plan monitoring and measurement approaches

### Phase 3: Detailed Planning Documentation
**Universal Frontend Stewardship**: Create comprehensive planning documentation that serves as implementation guide.

#### Planning Document Template
```markdown
# [Feature/Bug/Issue] Planning Document

## Planning Summary
- **Planning Date**: [Date]
- **Planning Duration**: [Time spent]
- **Planning Status**: [In Progress/Complete/Blocked]
- **Next Review Date**: [Date]

## Executive Summary
- **Objective**: [What we're trying to achieve]
- **Success Criteria**: [How we measure success]
- **Key Deliverables**: [What will be delivered]
- **Timeline**: [Estimated completion date]

## Requirements Analysis
### Functional Requirements
- [ ] [Requirement 1]
- [ ] [Requirement 2]
- [ ] [Requirement 3]

### Non-Functional Requirements
- [ ] [Performance requirements]
- [ ] [Security requirements]
- [ ] [Accessibility requirements]
- [ ] [Browser compatibility requirements]

## Technical Architecture
### System Overview
[High-level system architecture description]

### Component Design
- **Component 1**: [Description and responsibilities]
- **Component 2**: [Description and responsibilities]
- **Component 3**: [Description and responsibilities]

### Data Flow
[Description of data flow through the system]

### API Design
- **Endpoint 1**: [Method, path, parameters, response]
- **Endpoint 2**: [Method, path, parameters, response]

## Implementation Plan
### Phase 1: [Phase Name]
- [ ] [Task 1]
- [ ] [Task 2]
- [ ] [Task 3]

### Phase 2: [Phase Name]
- [ ] [Task 1]
- [ ] [Task 2]
- [ ] [Task 3]

## Dependencies & Prerequisites
### External Dependencies
- [ ] [Dependency 1]
- [ ] [Dependency 2]

### Internal Dependencies
- [ ] [Dependency 1]
- [ ] [Dependency 2]

## Risk Assessment
### High Risk
- [ ] [Risk 1]: [Mitigation strategy]
- [ ] [Risk 2]: [Mitigation strategy]

### Medium Risk
- [ ] [Risk 1]: [Mitigation strategy]
- [ ] [Risk 2]: [Mitigation strategy]

## Testing Strategy
### Unit Testing
- [ ] [Test case 1]
- [ ] [Test case 2]

### Integration Testing
- [ ] [Test case 1]
- [ ] [Test case 2]

### E2E Testing
- [ ] [Test case 1]
- [ ] [Test case 2]

## Outstanding Questions
### Critical Questions
- [ ] [Question 1]: [Why this is critical]
- [ ] [Question 2]: [Why this is critical]

### Important Questions
- [ ] [Question 1]: [Why this is important]
- [ ] [Question 2]: [Why this is important]

### Nice-to-Have Questions
- [ ] [Question 1]: [Why this is nice to have]
- [ ] [Question 2]: [Why this is nice to have]

## Next Steps
1. [Next step 1]
2. [Next step 2]
3. [Next step 3]

## Planning History
- **[Date]**: [What was planned/updated]
- **[Date]**: [What was planned/updated]
```

### Phase 4: Planning Validation & Quality Assurance
**Multi-Layer Verification**: Ensure comprehensive planning and clear documentation.

#### Planning Quality Gates
- **Completeness Check**: Verify all important aspects are planned
- **Technical Feasibility**: Confirm planned approach is technically sound
- **Resource Estimation**: Verify effort estimates are realistic
- **Risk Assessment**: Ensure all major risks are identified
- **Dependency Mapping**: Confirm all dependencies are identified
- **Testing Coverage**: Verify comprehensive testing strategy

#### Planning Review Process
- **Self-Review**: Review planning document for completeness and accuracy
- **Stakeholder Review**: Identify stakeholders who should review planning
- **Technical Review**: Ensure technical approach is sound
- **Business Review**: Verify business requirements are met
- **Implementation Readiness**: Confirm planning is ready for implementation

## Usage Examples

### Creating Planning Documents
```bash
# Create planning folder and document
mkdir -p planning
PLANNING_DOC="planning/feature-name-planning.md"
echo "# Feature Name Planning Document" > "$PLANNING_DOC"

# Add planning sections
echo "## Planning Summary" >> "$PLANNING_DOC"
echo "## Executive Summary" >> "$PLANNING_DOC"
echo "## Requirements Analysis" >> "$PLANNING_DOC"
```

### Planning Document Commands
- `/planning` - Execute comprehensive planning for current task
- `/planning --feature` - Focus on feature development planning
- `/planning --bug-fix` - Focus on bug fix planning
- `/planning --technical-debt` - Focus on technical debt planning
- `/planning @src/components/` - Plan specific component directory
- `/planning --validation` - Focus on planning validation and review
