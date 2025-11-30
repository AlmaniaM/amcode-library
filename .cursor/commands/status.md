# Status Command

## Purpose
Track, manage, and report implementation status across multiple LLM sessions to ensure seamless continuity and handoff between different AI agents working on the same project.

## When to Use
- **Session Handoff**: When starting a new LLM session and need to understand current status
- **Progress Monitoring**: When monitoring implementation progress and quality gates
- **Blocker Management**: When tracking and resolving implementation blockers
- **Status Reporting**: When providing status updates to stakeholders
- **Context Recovery**: When recovering context from previous sessions

## Command Structure

### Phase 1: Status Assessment & Context Recovery
**AUTONOMOUS PRINCIPAL ENGINEER DOCTRINE**: Begin with comprehensive assessment of current implementation status and recovery of context from previous sessions.

#### Status Assessment
- **Previous Session Review**: Review work completed in previous LLM sessions
- **Current State Analysis**: Analyze current implementation status and progress
- **Quality Gate Status**: Assess status of all quality gates and validation checkpoints
- **Blocker Identification**: Identify current blockers and unresolved issues
- **Dependency Status**: Check status of all dependencies and prerequisites

#### Context Recovery
- **Decision Rationale**: Recover decision rationale from previous sessions
- **Implementation Context**: Understand current implementation context and state
- **Technical Context**: Recover technical context and architectural decisions
- **Process Context**: Understand current process state and methodology
- **Environment Context**: Recover environment and configuration context

### Phase 2: Status Documentation & Tracking
**Comprehensive Status Management**: Document and track all aspects of implementation status.

#### Implementation Status Tracking
- **Task Status**: Track completion status of all planned tasks
- **Milestone Status**: Track progress against planned milestones
- **Quality Status**: Track status of all quality gates and validation checkpoints
- **Issue Status**: Track status of all issues, blockers, and resolutions
- **Dependency Status**: Track status of all dependencies and prerequisites

#### Progress Documentation
- **Session Summary**: Document what was completed in each session
- **Progress Metrics**: Calculate and document progress metrics
- **Quality Metrics**: Document quality metrics and validation results
- **Issue Metrics**: Document issue resolution metrics and trends
- **Performance Metrics**: Document performance and efficiency metrics

### Phase 3: Blocker Management & Resolution
**Systematic Blocker Resolution**: Identify, analyze, and resolve implementation blockers.

#### Blocker Identification
- **Active Blockers**: Identify current active blockers
- **Blocker Analysis**: Analyze root causes of blockers
- **Impact Assessment**: Assess impact of blockers on project timeline
- **Resolution Planning**: Plan resolution strategies for blockers
- **Escalation Planning**: Plan escalation procedures for critical blockers

#### Blocker Resolution
- **Resolution Implementation**: Implement resolution strategies
- **Resolution Testing**: Test and validate blocker resolutions
- **Resolution Documentation**: Document resolution process and outcomes
- **Prevention Planning**: Plan prevention strategies for similar blockers
- **Knowledge Transfer**: Transfer blocker resolution knowledge

### Phase 4: Status Reporting & Communication
**Comprehensive Status Communication**: Provide clear status reports and communication.

#### Status Report Generation
- **Executive Summary**: High-level status summary for stakeholders
- **Technical Details**: Detailed technical status for implementers
- **Progress Visualization**: Visual representation of progress and status
- **Risk Assessment**: Current risk assessment and mitigation status
- **Next Steps**: Clear next steps and recommendations

#### Stakeholder Communication
- **Status Updates**: Regular status updates to stakeholders
- **Progress Reports**: Detailed progress reports and metrics
- **Issue Communication**: Communication about issues and blockers
- **Milestone Updates**: Updates on milestone progress and completion
- **Quality Updates**: Updates on quality gate status and validation

### Phase 5: Session Handoff Preparation
**Seamless Session Handoff**: Prepare comprehensive handoff for subsequent LLM sessions.

#### Handoff Documentation
- **Session Summary**: Complete summary of current session work
- **Context Preservation**: Preserve all relevant context and decisions
- **Next Steps Definition**: Clearly define next steps for subsequent sessions
- **Blocker Handoff**: Document all blockers and resolution status
- **Knowledge Transfer**: Transfer all session-specific knowledge

#### Handoff Artifacts
- **Implementation Status Report**: Current status of all implementation tasks
- **Progress Summary**: Summary of progress made in current session
- **Quality Gate Status**: Status of all quality gates and validation checkpoints
- **Issue Log**: Complete log of all issues, resolutions, and remaining blockers
- **Context Documentation**: Complete context and decision rationale
- **Next Session Brief**: Clear brief for the next LLM session

## Status Tracking Methodologies

### Real-Time Status Tracking
- **Continuous Monitoring**: Monitor status continuously during implementation
- **Automated Updates**: Use automated tools to update status
- **Real-Time Alerts**: Set up alerts for status changes and issues
- **Dashboard Integration**: Integrate with project dashboards and tools
- **API Integration**: Use APIs to integrate with project management tools

### Session-Based Status Tracking
- **Session Start**: Assess status at the beginning of each session
- **Session Progress**: Track progress throughout the session
- **Session End**: Document status at the end of each session
- **Session Handoff**: Prepare handoff for next session
- **Session Review**: Review status across multiple sessions

### Milestone-Based Status Tracking
- **Milestone Definition**: Define clear milestones and deliverables
- **Milestone Progress**: Track progress toward milestones
- **Milestone Validation**: Validate milestone completion
- **Milestone Reporting**: Report on milestone status and completion
- **Milestone Adjustment**: Adjust milestones based on progress and changes

## Status Tracking Tools & Integration

### Project Management Integration
- **Jira Integration**: Integrate with Jira for task and issue tracking
- **Linear Integration**: Integrate with Linear for project management
- **GitHub Integration**: Integrate with GitHub for code and issue tracking
- **Azure DevOps Integration**: Integrate with Azure DevOps for project management
- **Custom Tool Integration**: Integrate with custom project management tools

### Status Tracking Formats
- **Markdown Reports**: Generate status reports in Markdown format
- **JSON Data**: Export status data in JSON format for tool integration
- **CSV Export**: Export status data in CSV format for analysis
- **Dashboard Integration**: Integrate with project dashboards
- **API Endpoints**: Provide API endpoints for status data access

## Status Tracking Outputs

### Status Artifacts
- **Implementation Status Report**: Comprehensive status report
- **Progress Summary**: Summary of progress and metrics
- **Quality Gate Status**: Status of all quality gates
- **Issue Log**: Complete log of issues and resolutions
- **Blocker Report**: Report on current blockers and resolution status
- **Context Documentation**: Complete context and decision documentation

### Quality Metrics
- **Task Completion Rate**: Percentage of tasks completed
- **Quality Gate Pass Rate**: Percentage of quality gates passed
- **Issue Resolution Rate**: Rate of issue resolution
- **Blocker Resolution Time**: Average time to resolve blockers
- **Session Productivity**: Productivity metrics per session
- **Overall Progress**: Overall project progress metrics

### Recommendations
- **Process Improvements**: Recommendations for improving processes
- **Tool Enhancements**: Recommendations for enhancing tools
- **Resource Allocation**: Recommendations for resource allocation
- **Risk Mitigation**: Recommendations for risk mitigation
- **Quality Improvements**: Recommendations for improving quality

## Usage Examples

### Session Handoff
```
/status
Review current implementation status and prepare handoff for next LLM session. Include context recovery and next steps definition.
```

### Progress Monitoring
```
/status
Generate comprehensive status report including progress metrics, quality gate status, and blocker analysis.
```

### Blocker Management
```
/status
Identify and analyze current blockers, plan resolution strategies, and update blocker status.
```

### Stakeholder Reporting
```
/status
Generate executive summary and detailed status report for stakeholder communication.
```

### Context Recovery
```
/status
Recover context from previous sessions and assess current implementation state for continuation.
```

## Success Criteria

### Status Tracking Quality
- **Completeness**: All aspects of implementation are tracked
- **Accuracy**: Status information is accurate and up-to-date
- **Clarity**: Status reports are clear and understandable
- **Actionability**: Status information leads to clear next steps
- **Timeliness**: Status information is current and timely

### Session Continuity
- **Seamless Handoff**: Smooth handoff between LLM sessions
- **Context Preservation**: Complete context preservation across sessions
- **Progress Continuity**: Continuous progress tracking across sessions
- **Knowledge Transfer**: Effective knowledge transfer between sessions
- **Quality Maintenance**: Quality maintained across all sessions

## Integration with Other Commands

### Pre-Implementation
- Use with `/plan` to establish status tracking framework
- Use with `/understand` to build context before status assessment
- Use with `/analyze-project-deep` to understand project state

### During Implementation
- Use with `/implement` to track implementation progress
- Use with `/test` to track testing and validation status
- Use with `/debug` to track debugging and issue resolution

### Post-Implementation
- Use with `/retro` to analyze status tracking effectiveness
- Use with `/plan` to plan improvements based on status analysis
- Use with `/implement` to continue work based on status assessment

**Remember**: The `/status` command is your continuity engine for multi-session development. Use it to track implementation status, manage blockers, and ensure seamless handoff between LLM sessions. The command adapts to your specific needs and provides the level of status tracking detail appropriate for your context.
