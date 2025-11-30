# Save Plan Command

## Overview
Save detailed planning context with comprehensive progress tracking, task breakdowns, and AI session handoffs. Creates structured planning documents similar to Cursor planning mode that enable seamless continuation across AI sessions.

## Mission Briefing: Planning Document Protocol

You will now transition to **Planning Archivist** mode. Your mission is to capture and document the current state of work in a detailed, structured format that enables seamless continuation in future sessions.

**Your goal is to create or update a comprehensive planning document that preserves all critical information for future reference and provides clear handoff to the next AI session.**

---

## **CRITICAL DIRECTIVE: Planning Document Updates**

**This command creates or updates planning documents (`.plan.md` files):**

1. **CHECK if planning document exists** in `planning/[ticket-id]/` directory
2. **UPDATE existing planning document** with current progress OR
3. **CREATE new planning document** if none exists
4. **ENSURE all progress tracking is current** and detailed
5. **PROVIDE clear handoff** for next AI session

**Remember**: The planning document is the PRIMARY source of truth for all implementation work.

---

## **Planning Capture Protocol**

**Directive:** Analyze the current session and capture the essential planning context in the following structured format:

### **Pre-Capture Setup**

1. **Identify Planning Document:** Look for existing `.plan.md` files in `planning/` directory
2. **Determine File Path:** 
   - Existing: Use found planning document path
   - New: `planning/[ticket-id]/[feature-name].plan.md`
3. **Get Current State:** Review all completed and pending work
4. **Gather Context:** Collect file changes, decisions, blockers, next steps

### **Planning Document Structure**

Create or update planning document with the following sections:

#### 1. Progress Tracking (REQUIRED)

**Status indicators, current phase, implementation progress table, active work, and detailed task breakdown**

#### 2. Implementation Strategy (REQUIRED)

**High-level approach, architecture decisions, key patterns to follow**

#### 3. Technical Context (REQUIRED)

**Files involved, dependencies, integration points, code patterns**

#### 4. Testing Strategy (if applicable)

**Unit tests, E2E tests, verification approaches**

#### 5. What We've Done (REQUIRED)

**Completed tasks, files modified, key decisions, progress metrics**

#### 6. What Worked (REQUIRED)

**Successful patterns, effective approaches, problem solutions**

#### 7. What Didn't Work (REQUIRED)

**Failed approaches, error patterns, blockers, lessons learned**

---

## **Output Format**

Create or update a planning document with the following structure:

```markdown
# [Feature Name] - Planning Document

## Progress Tracking

**Status**: [Planning Complete | In Progress | Blocked | Complete]

**Last Updated**: [Date and Time]

**Current Phase**: [Phase Name/Number]

### Implementation Progress

| Phase | Status | Completed Tasks | Notes |
|-------|--------|-----------------|-------|
| Phase 1: [Name] | ✅ Complete | 5/5 | [Brief note] |
| Phase 2: [Name] | 🔄 In Progress | 3/5 | Currently on Task 2.4 |
| Phase 3: [Name] | ⏸️ Not Started | 0/4 | Waiting for Phase 2 |

**Legend**: ⏸️ Not Started | 🔄 In Progress | ✅ Complete | ⚠️ Blocked

### Active Work

**Currently Working On**: 
[Specific task with file path and line number if applicable]

**Progress Details**:
- ✅ [Completed sub-item]
- 🔄 [Current sub-item with details]
- ⏸️ [Next sub-item]

**Blockers**: [Description or "None"]

**Next Steps**:
1. [Immediate next action with file/location]
2. [Following action]
3. [Subsequent action]

### What's Left (Detailed Breakdown)

#### Phase 1: [Phase Name]
- [x] Task 1.1: [Completed task with brief note]
- [x] Task 1.2: [Completed task]
- [ ] Task 1.3: [Current/next task] - 🔄 In Progress
  - File: `path/to/file.ts`
  - Current state: [Details]
  - Next step: [Specific action]
- [ ] Task 1.4: [Remaining task] - ⏸️ Not Started
  - Prerequisites: [What needs to be done first]
  - Estimated time: 15-20 min

#### Phase 2: [Phase Name]
- [ ] Task 2.1: [Description] - ⏸️ Not Started
  - Dependencies: Complete Phase 1
  - Estimated time: 10 min

---

## Implementation Strategy

[High-level description of the approach]

**Key Architecture Decisions:**
- Decision 1: [Description and rationale]
- Decision 2: [Description and rationale]

**Patterns to Follow:**
- Pattern 1: [Description and where to find it]
- Pattern 2: [Description and reference]

**Integration Points:**
- Component/Service 1: [How it integrates]
- Component/Service 2: [How it integrates]

---

## Technical Context

### Files Involved

**Primary Files:**
- `path/to/file1.ts` - [Purpose and changes]
- `path/to/file2.ts` - [Purpose and changes]

**Supporting Files:**
- `path/to/file3.ts` - [Purpose]

### Dependencies

**Required Services:**
- Service 1: [How it's used]
- Service 2: [How it's used]

**Required Libraries:**
- Library 1: [Version and purpose]

### Code Patterns

**Pattern 1: [Name]**
```typescript
// Example or reference
// Location: file.ts:123-145
```

**Pattern 2: [Name]**
- Reference: `file.ts:200-250`
- Description: [What this pattern does]

---

## Testing Strategy

### Unit Tests

**Files to Test:**
- `file1.spec.ts` - [What to test]
- `file2.spec.ts` - [What to test]

**Test Cases:**
1. Test Case 1: [Description]
2. Test Case 2: [Description]

### E2E Tests

**Test Files:**
- `test1.e2e.ts` - [Scenario]
- `test2.e2e.ts` - [Scenario]

**Test Scenarios:**
1. Scenario 1: [Description]
2. Scenario 2: [Description]

---

## What We've Done

### Completed Tasks
- [x] **Task 1**: [Description with outcome]
  - File: `path/to/file.ts`
  - Changes: [Brief description]
  - Verified: [How it was verified]

### Files Modified

**Created:**
- `path/to/new-file1.ts` - [Purpose]

**Modified:**
- `path/to/file1.tsx` - [Changes made]

**Deleted:**
- `path/to/old-file.ts` - [Reason]

### Key Decisions
- **Decision 1**: [Description]
  - **Rationale**: [Why this decision was made]
  - **Impact**: [What this affects]
  - **Date**: [When]

### Progress Metrics
- Overall progress: X%
- Phases complete: X/Y
- Tests passing: X/Y

---

## What Worked

### Successful Patterns
- **Pattern 1**: [Description]
  - **Why it worked**: [Explanation]
  - **Code reference**: `file.ts:123-145`

### Effective Approaches
- **Approach 1**: [How it helped and when to use again]

### Problem Solutions
- **Problem 1**: [Problem description]
  - **Solution**: [How it was solved]
  - **Outcome**: [Result]

---

## What Didn't Work

### Failed Approaches
- **Approach 1**: [What was tried]
  - **Why it failed**: [Root cause]
  - **What was learned**: [Lesson]
  - **Alternative taken**: [What worked instead]

### Error Patterns
- **Error 1**: [Error description]
  - **Frequency**: [How often encountered]
  - **Resolution**: [How resolved or workaround]

### Blockers
- **Blocker 1**: [Description]
  - **Impact**: [What's blocked]
  - **Resolution approach**: [Plan to resolve]
  - **Status**: [Current state]

---

## Handoff Checklist for Next AI

- [ ] Planning document updated with latest progress
- [ ] All completed tasks marked with ✅
- [ ] "Currently Working On" is specific and actionable
- [ ] Next 2-3 steps are clear and detailed
- [ ] Any blockers are documented with context
- [ ] File paths and line numbers provided
- [ ] Code patterns/references documented
- [ ] Verification approach specified
- [ ] Dependencies identified

**Next AI**: Start by reading this planning document first for complete context.
```

---

## **File Naming Convention**

**Primary Location:** `planning/[ticket-id]/` directory in workspace root

**Filename Pattern:** `[feature-name].plan.md`

**Examples:**
- `planning/aae-38752/subscription-aware-palette-filtering.plan.md`
- `planning/xat-12345/log-history-improvements.plan.md`
- `planning/bug-fix-123/connector-validation-fix.plan.md`

**Directory Structure:**
```
planning/
  ├── aae-38752/
  │   ├── subscription-aware-palette-filtering.plan.md
  │   └── implementation-notes.md (optional supporting docs)
  ├── xat-12345/
  │   └── feature.plan.md
```

---

## **Usage Examples & When to Use**

### Required Usage

- **Starting new feature work** - Create initial planning document
- **After each completed task** - Update progress tracking
- **Before ending development session** - Update with current state
- **When making key decisions** - Document decisions and rationale
- **When hitting blockers** - Document blocker and investigation

### Recommended Usage

- **After completing each phase** - Update phase status and metrics
- **When changing approach** - Document why and what changed
- **Every hour of active work** - Keep progress tracking current
- **Before context switch** - Ensure work can be resumed

### Command Usage

```bash
/save-plan
```

**The command will:**
1. Find or create appropriate planning document
2. Update progress tracking with current state
3. Mark completed tasks and update counters
4. Update "Currently Working On" section
5. Ensure "Next Steps" are clear and actionable
6. Include handoff checklist for next AI

---

## **Task Granularity Requirements**

Break down work into **10-20 minute implementable chunks:**

✅ **GOOD Task Granularity:**
- Tasks completable in 10-20 minutes
- Single file or small group of related files
- Clear start and end conditions
- Can be verified with a test or visual check

❌ **TOO LARGE (Break Down Further):**
- "Implement entire feature"
- "Add all unit tests"
- "Complete Phase X"

**Example Breakdown:**
```
BAD:  - [ ] Implement connector filtering

GOOD: - [ ] Task 2.1: Inject StudioSubscriptionVisibilityService in HxpPaletteService
      - [ ] Task 2.2: Add filterConnectorsBySubscription() private method  
      - [ ] Task 2.3: Update handleConnectorsSubscription() to call filtering
      - [ ] Task 2.4: Add error handling with catchError
      - [ ] Task 2.5: Verify with manual test
```

---

## **Continuous Update Workflow**

### During Active Development:

**Starting a Task:**
- Mark task as 🔄 In Progress
- Update "Currently Working On" with file/line
- Update "Last Updated" timestamp
- Run `/save-plan`

**Completing a Task:**
- Mark task as ✅ Complete
- Update task counter (e.g., 3/5 → 4/5)
- Add brief completion note
- Update "Currently Working On" to next task
- Update "Last Updated" timestamp
- Run `/save-plan`

**Hitting a Blocker:**
- Mark task/phase as ⚠️ Blocked
- Document blocker in detail
- Update "Next Steps" with resolution approach
- Update "Last Updated" timestamp
- Run `/save-plan`

**Making a Decision:**
- Add to "Key Decisions" or "Decisions Made" log
- Update relevant tasks if approach changed
- Update "Last Updated" timestamp
- Run `/save-plan`

---

## **Integration with Other Commands**

This command works with:
- `/save-context` - For supplementary narrative and session context
- `/retro` - For learning capture and doctrine evolution
- `/analyze` - For project state analysis
- `/do` - For task execution tracking

**Key Differences:**
- `/save-plan` → PRIMARY source of truth for progress tracking and planning
- `/save-context` → SUPPLEMENTARY narrative and session details
- `/retro` → Learnings and doctrine evolution (different focus)

---

## **Next AI Session Handoff**

When the next AI picks up work, they should:

1. **Read planning document first** (in `planning/[ticket-id]/`)
2. **Check "Currently Working On"** for immediate task
3. **Review "Next Steps"** for action items
4. **Verify no blockers** or understand blocker context
5. **Check phase status** to understand overall progress
6. **Reference code patterns** for implementation guidance
7. **Continue from checkpoint** with full context

This ensures seamless continuation across AI sessions with complete context.

---

## **Success Criteria**

A well-maintained planning document allows ANY AI to:

- [ ] Understand what work has been completed
- [ ] Know exactly what to work on next
- [ ] See the context and reasoning for past decisions
- [ ] Identify and understand any blockers
- [ ] Find code references and patterns to follow
- [ ] Continue work without needing to search or investigate
- [ ] Verify their work matches the plan

**Remember**: The planning document is the lifeline between AI sessions. Keep it updated, detailed, and actionable.

