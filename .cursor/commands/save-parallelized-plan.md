# Save Parallelized Plan Command

## Purpose
Combine `/save-plan` and `/parallelize` commands to create a comprehensive parallelized plan with progress tracking, coordination files, and ready-to-execute phase structure. This command saves the current planning context AND parallelizes it into independent, executable phases.

## When to Use
- **Large-Scale Features**: When you have a comprehensive plan that needs parallel execution
- **Multi-LLM Coordination**: When multiple LLMs will work on the same project
- **Complex Implementations**: When breaking down work into independent phases
- **Planning + Parallelization**: When you want to save progress AND create parallel structure in one step

## Command Structure

### Phase 1: Planning Context Capture
**Follow `/save-plan` protocol**: Capture current planning state and progress.

#### Planning Document Creation
- **Check for Existing Plan**: Look for existing `.plan.md` files
- **Create or Update**: Create new plan or update existing with current progress
- **Progress Tracking**: Document completed tasks, current work, blockers
- **Context Preservation**: Save all decisions, patterns, and learnings

### Phase 2: Plan Analysis & Parallelization
**Follow `/parallelize` protocol**: Break down plan into independent phases.

#### Dependency Analysis
- **Task Dependency Mapping**: Map all task dependencies
- **Resource Requirements**: Identify resource needs
- **Critical Path Analysis**: Identify parallelization opportunities
- **Phase Independence**: Ensure phases have zero dependencies

#### Phase Creation
- **Independent Phases**: Create phases that can execute in parallel
- **Clear Boundaries**: Define phase boundaries and interfaces
- **Balanced Workload**: Balance complexity and effort across phases
- **Integration Planning**: Plan how phases integrate

### Phase 3: Coordination Files Creation
**MANDATORY**: Create coordination files for multi-LLM execution.

#### Required Coordination Files
- **LLM-EXECUTION-PROMPT.md**: Concise execution instructions (~100 lines)
- **STATUS.md**: Real-time status tracking for coordination
- **QUICK-START.md**: Quick start guide for coordinators
- **integration-guide.md**: Integration guide for combining phases
- **quality-checklist.md**: Quality checklist for all phases

### Phase 4: Document Organization
**Structured Output**: Organize all documents in parallelized structure.

#### Directory Structure
```
planning/[plan-name]/
├── [plan-name].plan.md              # Original plan with progress tracking
├── README.md                        # Master index and overview
├── LLM-EXECUTION-PROMPT.md         # Execution prompt (MANDATORY)
├── STATUS.md                        # Real-time status tracking (MANDATORY)
├── QUICK-START.md                   # Quick start guide
├── integration-guide.md             # Integration guide
├── quality-checklist.md             # Quality checklist
├── phase-1-[phase-name]/
│   └── phase-plan.md
├── phase-2-[phase-name]/
│   └── phase-plan.md
└── [additional-phases...]
```

## Execution Flow

### Step 1: Capture Planning Context
1. Analyze current session work
2. Document completed tasks and progress
3. Capture decisions and learnings
4. Identify blockers and next steps
5. Create/update planning document with progress tracking

### Step 2: Analyze for Parallelization
1. Map all task dependencies
2. Identify independent workstreams
3. Create phase boundaries
4. Balance workload across phases
5. Define integration points

### Step 3: Create Parallel Structure
1. Create phase directories
2. Create phase plans with detailed tasks
3. Create coordination files (LLM-EXECUTION-PROMPT.md, STATUS.md, QUICK-START.md)
4. Create integration guide
5. Create quality checklist

### Step 4: Finalize Documentation
1. Update master README with all phases
2. Ensure all coordination files are complete
3. Verify phase independence
4. Document integration strategy

## Coordination Files Content

**Reference**: See `.cursor/commands/parallelize.md` for detailed coordination file templates.

### LLM-EXECUTION-PROMPT.md
**Content** (see parallelize.md for full template):
- **Autonomous Phase Selection** (MANDATORY): Instructions for LLMs to autonomously select phases based on:
  - Availability (not in progress, dependencies met, not blocked)
  - Priority (phases that unblock others, critical path)
  - Task availability (phases with incomplete tasks)
- Quick start steps (select phase, check STATUS.md, read phase plan, update status)
- Real-time status update requirements (MANDATORY)
- Code quality principles (SOLID/DRY/YAGNI)
- `/understand` command usage examples
- Phase-specific implementation guidelines
- Testing and documentation requirements
- Completion checklist
- Next phase selection after completion

**Format**: Concise (~100 lines), actionable, phase-agnostic template

### STATUS.md
**Content** (see parallelize.md for full template):
- Active work tracking table (phase, LLM, task, file, status, timestamp, ETA)
- Task-level status for all phases (checkboxes)
- Blockers section
- Integration points tracking
- Coordination notes section

**Format**: Table-based, easy to update, real-time coordination

### QUICK-START.md
**Content** (see parallelize.md for full template):
- How to assign phases to LLMs
- How to provide execution prompt
- How to monitor progress
- Integration steps
- File structure overview

**Format**: Quick reference for coordinators

## Output Structure

### Planning Document
- **Location**: `planning/[plan-name]/[plan-name].plan.md`
- **Content**: Full planning context with progress tracking (from `/save-plan`)
- **Purpose**: Primary source of truth for planning state

### Parallelized Structure
- **Location**: `planning/[plan-name]/`
- **Content**: Phase directories, coordination files, integration guides
- **Purpose**: Ready-to-execute parallel structure

## Success Criteria

### Planning Quality
- [ ] All current progress captured
- [ ] All decisions documented
- [ ] All blockers identified
- [ ] Next steps clear

### Parallelization Quality
- [ ] Phases are independent (zero dependencies)
- [ ] Phases are balanced (workload, complexity)
- [ ] Phase boundaries are clear
- [ ] Integration is well-planned

### Coordination Quality
- [ ] LLM-EXECUTION-PROMPT.md includes autonomous phase selection instructions
- [ ] LLM-EXECUTION-PROMPT.md is concise and actionable
- [ ] STATUS.md enables real-time coordination
- [ ] QUICK-START.md is clear for coordinators
- [ ] Phase dependencies are clearly documented for autonomous selection
- [ ] All files are ready for immediate use

## Usage Examples

### Basic Usage
```
/save-parallelized-plan
Save the current PaddleOCR integration plan and parallelize it into independent phases.
```

### With Context
```
/save-parallelized-plan
I've been working on the PaddleOCR Python service. Save current progress and create parallelized structure for Python service, .NET integration, local dev setup, debugging, and deployment phases.
```

## Integration with Other Commands

### Pre-Execution
- Use after `/plan` to save and parallelize comprehensive plans
- Use after `/understand` to save understanding and create parallel structure

### Post-Execution
- Use with `/status` to track progress across phases
- Use with `/retro` to capture learnings from parallel execution

## Key Differences from Individual Commands

### vs `/save-plan`
- **Additional**: Creates parallelized phase structure
- **Additional**: Creates coordination files
- **Additional**: Enables multi-LLM execution

### vs `/parallelize`
- **Additional**: Captures current planning context first
- **Additional**: Includes progress tracking in planning document
- **Additional**: Preserves session learnings and decisions

## Critical Requirements

### MANDATORY Coordination Files
1. **LLM-EXECUTION-PROMPT.md**: Must be created with concise, actionable instructions
2. **STATUS.md**: Must be created with real-time tracking structure
3. **QUICK-START.md**: Must be created for coordinator reference

### MANDATORY Autonomous Phase Selection
- LLMs must autonomously select phases based on availability and priority
- Selection criteria: availability, dependencies, blockers, critical path
- Must claim phase in STATUS.md before starting work
- Required for efficient parallel execution without manual assignment

### MANDATORY Status Updates
- LLMs must update STATUS.md after each task
- Prevents conflicts and enables coordination
- Required for successful parallel execution

### MANDATORY Phase Independence
- Phases must have zero dependencies
- Can execute in parallel without coordination
- Integration happens after phases complete

---

**Remember**: This command combines planning preservation with parallelization. It creates a complete, ready-to-execute parallel structure with coordination mechanisms for multi-LLM development.

