# Debug Planning Protocol

## Overview
Execute systematic debugging planning and discovery tracking for complex issues. This command helps understand project files, runtime execution flows, and maintains comprehensive debugging documentation that automatically updates based on discoveries and solutions.

**Output Location**: All debug documentation files are saved in the `debug-planning/` folder for organized tracking and easy access.

## Mission Briefing: Debug Planning Protocol

**DEBUGGING COMMAND DETECTION:** Before proceeding with debugging planning, check if the provided text contains any `/analyze*` command (e.g., `/analyze-code`, `/analyze-project`, `/analyze-bug-*`, etc.). If found, skip this command's debugging and execute the detected command instead to avoid duplicate analysis.

You will now execute systematic debugging planning using the **AUTONOMOUS PRINCIPAL ENGINEER - OPERATIONAL DOCTRINE.** This debugging planning follows the Phase 0 reconnaissance principles with specialized focus on discovery tracking, solution evolution, and comprehensive debugging documentation.

---

## **Phase 0: Debug Planning Reconnaissance & Mental Modeling (Read-Only)**

**Directive:** Perform non-destructive analysis of current debugging state to build a complete understanding of issues, project structure, and debugging opportunities.

**Debug Planning Scope:**
- **Project File Analysis**: Understanding project structure, dependencies, and execution flows
- **Runtime Flow Mapping**: Tracing application execution paths and data flow
- **Issue Discovery Tracking**: Systematic tracking of what has been discovered, what works, what doesn't work
- **Solution Evolution**: Automatic updating of solutions based on new discoveries
- **Debug Documentation**: Creating and maintaining comprehensive debugging documentation

**Constraints:**
- **No mutations are permitted during this phase**
- **Focus solely on understanding current debugging state and planning approach**
- **Identify debugging opportunities without implementing fixes**

---

## **Phase 1: Project Structure & Runtime Flow Analysis**

**Directive:** Analyze project structure and understand runtime execution flows for debugging context.

**Project Analysis Areas:**

1. **Monorepo Structure Analysis:**
   - Application and library organization
   - Dependency relationships and circular dependencies
   - Build system and configuration files
   - Testing structure and coverage

2. **Runtime Execution Flow Mapping:**
   - Application startup and initialization flows
   - Component lifecycle and rendering flows
   - Service injection and data flow patterns
   - Route navigation and state management flows

3. **Technology Stack Analysis:**
   - Framework versions and compatibility
   - Build tools and configuration
   - Testing frameworks and utilities
   - Development and debugging tools

4. **File Organization Analysis:**
   - Source code organization patterns
   - Configuration file hierarchy
   - Asset management and optimization
   - Documentation and README structure

**Analysis Commands:**
```bash
# Project structure analysis
find . -name "*.ts" -o -name "*.js" -o -name "*.json" | head -20
ls -la apps/ libs/ config/

# Dependency analysis
npm list --depth=0
npm outdated

# Build system analysis
cat package.json | grep -A 10 -B 10 "scripts"
cat nx.json | head -20
cat tsconfig.json | head -20

# Runtime flow analysis
grep -r "bootstrap\|main\|start" apps/ --include="*.ts" | head -10
grep -r "NgModule\|Component\|Service" libs/ --include="*.ts" | head -10
```

---

## **Phase 2: Debug Discovery Tracking System**

**Directive:** Implement systematic tracking of debugging discoveries, solutions, and progress.

**Discovery Tracking Areas:**

1. **Issue Discovery Tracking:**
   - Current issues identified and their characteristics
   - Error patterns and recurring problems
   - Root cause analysis progress
   - Impact assessment and severity classification

2. **Solution Tracking:**
   - Approaches attempted and their outcomes
   - Successful solutions and their effectiveness
   - Failed approaches and lessons learned
   - Partial solutions and next steps

3. **Progress Tracking:**
   - Debugging milestones and achievements
   - Time spent on different debugging areas
   - Knowledge gained and documentation created
   - Tools and techniques discovered

4. **Knowledge Management:**
   - Debugging patterns and best practices identified
   - Common issues and their solutions
   - Tool effectiveness and usage patterns
   - Team knowledge and expertise areas

**Tracking Commands:**
```bash
# Create debug tracking file in debug-planning folder
mkdir -p debug-planning
DEBUG_FILE="debug-planning/debug-session-$(date +%Y%m%d-%H%M%S).md"
echo "# Debug Session - $(date)" > "$DEBUG_FILE"

# Track current issues
echo "## Current Issues" >> "$DEBUG_FILE"
echo "- [ ] Issue 1: Description" >> "$DEBUG_FILE"
echo "- [ ] Issue 2: Description" >> "$DEBUG_FILE"

# Track solutions attempted
echo "## Solutions Attempted" >> "$DEBUG_FILE"
echo "- [x] Solution 1: Description - Status" >> "$DEBUG_FILE"
echo "- [ ] Solution 2: Description - Status" >> "$DEBUG_FILE"
```

---

## **Phase 3: Automatic Solution Evolution System**

**Directive:** Implement system for automatically updating solutions based on new discoveries.

**Solution Evolution Areas:**

1. **Discovery Integration:**
   - Automatic integration of new discoveries into existing solutions
   - Pattern recognition and solution refinement
   - Cross-reference analysis and solution validation
   - Solution effectiveness tracking and optimization

2. **Solution Documentation:**
   - Automatic documentation of solution evolution
   - Version control for solution approaches
   - Success/failure tracking and analysis
   - Best practice identification and documentation

3. **Knowledge Base Updates:**
   - Automatic updates to debugging knowledge base
   - Pattern library maintenance and expansion
   - Tool effectiveness tracking and recommendations
   - Team knowledge sharing and collaboration

4. **Solution Validation:**
   - Automatic testing of solution effectiveness
   - Regression testing and validation
   - Performance impact assessment
   - User experience validation

**Evolution Commands:**
```bash
# Update solution based on new discovery
echo "## Solution Evolution - $(date)" >> "$DEBUG_FILE"
echo "### New Discovery: [Description]" >> "$DEBUG_FILE"
echo "### Updated Solution: [Description]" >> "$DEBUG_FILE"
echo "### Validation: [Results]" >> "$DEBUG_FILE"

# Track solution effectiveness
echo "## Solution Effectiveness" >> "$DEBUG_FILE"
echo "- Solution 1: [Effectiveness Rating] - [Notes]" >> "$DEBUG_FILE"
echo "- Solution 2: [Effectiveness Rating] - [Notes]" >> "$DEBUG_FILE"
```

---

## **Phase 4: Comprehensive Debug Documentation System**

**Directive:** Create and maintain comprehensive debugging documentation that automatically updates.

**Documentation Areas:**

1. **Debug Session Documentation:**
   - Session overview and objectives
   - Issues discovered and their characteristics
   - Solutions attempted and their outcomes
   - Lessons learned and best practices

2. **Project Debugging Guide:**
   - Project-specific debugging patterns
   - Common issues and their solutions
   - Tool usage and effectiveness
   - Team debugging procedures

3. **Solution Library:**
   - Catalog of successful solutions
   - Solution templates and patterns
   - Best practices and guidelines
   - Tool recommendations and usage

4. **Knowledge Base:**
   - Debugging methodology and approaches
   - Technology-specific debugging patterns
   - Common pitfalls and how to avoid them
   - Continuous improvement and learning

**Documentation Commands:**
```bash
# Create comprehensive debug documentation in debug-planning folder
mkdir -p debug-planning
DEBUG_DOC="debug-planning/debug-planning-$(date +%Y%m%d-%H%M%S).md"
echo "# Debug Planning Documentation - $(date)" > "$DEBUG_DOC"

# Add project analysis
echo "## Project Analysis" >> "$DEBUG_DOC"
echo "### Structure" >> "$DEBUG_DOC"
echo "### Runtime Flows" >> "$DEBUG_DOC"
echo "### Technology Stack" >> "$DEBUG_DOC"

# Add debugging tracking
echo "## Debugging Tracking" >> "$DEBUG_DOC"
echo "### Issues Discovered" >> "$DEBUG_DOC"
echo "### Solutions Attempted" >> "$DEBUG_DOC"
echo "### Progress Made" >> "$DEBUG_DOC"

# Add solution evolution
echo "## Solution Evolution" >> "$DEBUG_DOC"
echo "### Discovery Integration" >> "$DEBUG_DOC"
echo "### Solution Refinement" >> "$DEBUG_DOC"
echo "### Knowledge Base Updates" >> "$DEBUG_DOC"
```

---

## **Phase 5: Debug Planning Implementation**

**Directive:** Implement systematic debugging planning and discovery tracking.

**Implementation Steps:**

1. **Project Analysis Implementation:**
   - Execute project structure analysis
   - Map runtime execution flows
   - Identify debugging opportunities
   - Document findings and insights

2. **Discovery Tracking Implementation:**
   - Set up systematic discovery tracking
   - Implement solution tracking system
   - Create progress monitoring
   - Establish knowledge management

3. **Solution Evolution Implementation:**
   - Implement automatic solution updates
   - Set up solution validation
   - Create knowledge base updates
   - Establish continuous improvement

4. **Documentation Implementation:**
   - Create comprehensive debug documentation
   - Set up automatic documentation updates
   - Implement knowledge sharing
   - Establish best practices

**Implementation Commands:**
```bash
# Execute debug planning
npm run debug:planning
npm run debug:discovery
npm run debug:evolution
npm run debug:documentation

# Validate implementation
npm run debug:validate
npm run debug:test
npm run debug:verify
```

---

## **Phase 6: Debug Planning Validation & Testing**

**Directive:** Validate debugging planning implementation and test effectiveness.

**Validation Areas:**

1. **Project Analysis Validation:**
   - Verify project structure understanding
   - Validate runtime flow mapping
   - Test debugging opportunity identification
   - Confirm analysis completeness

2. **Discovery Tracking Validation:**
   - Test discovery tracking system
   - Validate solution tracking
   - Verify progress monitoring
   - Confirm knowledge management

3. **Solution Evolution Validation:**
   - Test automatic solution updates
   - Validate solution effectiveness
   - Verify knowledge base updates
   - Confirm continuous improvement

4. **Documentation Validation:**
   - Test documentation completeness
   - Validate automatic updates
   - Verify knowledge sharing
   - Confirm best practices

**Validation Commands:**
```bash
# Validate debug planning
npm run debug:validate:planning
npm run debug:validate:tracking
npm run debug:validate:evolution
npm run debug:validate:documentation

# Test effectiveness
npm run debug:test:effectiveness
npm run debug:test:usability
npm run debug:test:maintainability
```

---

## **Phase 7: Debug Planning Optimization & Maintenance**

**Directive:** Optimize debugging planning system and establish maintenance procedures.

**Optimization Areas:**

1. **System Performance Optimization:**
   - Optimize discovery tracking performance
   - Improve solution evolution efficiency
   - Enhance documentation generation
   - Streamline knowledge management

2. **User Experience Optimization:**
   - Improve debugging workflow
   - Enhance discovery tracking usability
   - Optimize solution evolution interface
   - Streamline documentation access

3. **Maintenance Procedures:**
   - Establish regular maintenance schedules
   - Create update and upgrade procedures
   - Implement monitoring and alerting
   - Establish backup and recovery

4. **Continuous Improvement:**
   - Monitor system effectiveness
   - Collect user feedback
   - Implement improvements
   - Share learnings and best practices

**Optimization Commands:**
```bash
# Optimize debug planning
npm run debug:optimize:performance
npm run debug:optimize:usability
npm run debug:optimize:maintenance

# Monitor effectiveness
npm run debug:monitor:effectiveness
npm run debug:monitor:usage
npm run debug:monitor:feedback
```

---

## **Phase 8: Debug Planning Documentation & Knowledge Sharing**

**Directive:** Create comprehensive documentation and establish knowledge sharing.

**Documentation Areas:**

1. **User Guide:**
   - How to use debug planning system
   - Discovery tracking procedures
   - Solution evolution workflows
   - Documentation maintenance

2. **Technical Documentation:**
   - System architecture and design
   - Implementation details and patterns
   - API documentation and usage
   - Troubleshooting and maintenance

3. **Best Practices Guide:**
   - Debugging methodology and approaches
   - Discovery tracking best practices
   - Solution evolution patterns
   - Knowledge management strategies

4. **Team Collaboration:**
   - Team debugging procedures
   - Knowledge sharing protocols
   - Collaboration tools and workflows
   - Continuous learning and improvement

**Documentation Commands:**
```bash
# Create user documentation
npm run debug:docs:user
npm run debug:docs:technical
npm run debug:docs:best-practices
npm run debug:docs:team

# Generate knowledge base
npm run debug:knowledge:generate
npm run debug:knowledge:update
npm run debug:knowledge:share
```

---

## **Usage Examples**

- `/debug-planning` - Execute comprehensive debugging planning and discovery tracking
- `/debug-planning --project-analysis` - Focus on project structure and runtime flow analysis
- `/debug-planning --discovery-tracking` - Focus on discovery tracking and solution evolution
- `/debug-planning --documentation` - Focus on comprehensive debug documentation
- `/debug-planning @src/components/` - Debug plan specific component directory
- `/debug-planning --runtime-flows` - Focus on runtime execution flow analysis
- `/debug-planning --solution-evolution` - Focus on automatic solution evolution
- `/debug-planning --knowledge-base` - Focus on knowledge base management

## **Debug Planning Features**

- **Project Analysis**: Comprehensive project structure and runtime flow understanding
- **Discovery Tracking**: Systematic tracking of debugging discoveries and progress
- **Solution Evolution**: Automatic updating of solutions based on new discoveries
- **Debug Documentation**: Comprehensive debugging documentation with automatic updates
- **Knowledge Management**: Systematic knowledge capture and sharing
- **Progress Monitoring**: Real-time tracking of debugging progress and effectiveness
- **Best Practices**: Identification and documentation of debugging best practices
- **Team Collaboration**: Knowledge sharing and collaborative debugging procedures

**Remember**: This command provides comprehensive debugging planning capabilities to systematically understand project files, track discoveries, evolve solutions, and maintain comprehensive debugging documentation that automatically updates based on new findings and solutions.
