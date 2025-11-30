# Save Context Command

## Overview
Save the current LLM context with session status, progress, and next steps. Use this to maintain continuity across sessions and preserve important context about ongoing work.

## Mission Briefing: Context Preservation Protocol

You will now transition to **Context Archivist** mode. Your mission is to capture and document the current state of work in a structured format that enables seamless continuation in future sessions.

**Your goal is to create a comprehensive snapshot of the current session that preserves all critical information for future reference.**

---

## **Context Capture Protocol**

**Directive:** Analyze the current session and capture the essential context in the following structured format:

### **Pre-Capture Setup**
1. **Get Chat Title:** Identify the current chat window title
2. **Create Filename:** Convert title to lowercase and replace spaces with dashes
3. **Ensure Directory:** Verify `C:\dev\cursor\saved-context` exists, create if needed
4. **Generate Full Path:** `C:\dev\cursor\saved-context\[converted-title].md`

### **1. What We've Done**
- **Completed Tasks:** List all tasks, migrations, fixes, or implementations completed in this session
- **Files Modified:** Document all files that were created, edited, or deleted
- **Key Decisions:** Record important architectural or implementation decisions made
- **Progress Metrics:** Note any progress percentages, completion status, or milestones reached

### **2. What Worked**
- **Successful Patterns:** Document approaches, patterns, or techniques that worked well
- **Effective Tools:** Note which tools, commands, or methodologies were effective
- **Problem Solutions:** Record successful solutions to problems encountered
- **Best Practices:** Capture any best practices discovered or validated

### **3. What Didn't Work**
- **Failed Approaches:** Document approaches that failed and why they failed
- **Error Patterns:** Record recurring errors or issues encountered
- **Blockers:** Note any blockers or obstacles that prevented progress
- **Lessons Learned:** Capture specific lessons from failures or challenges

### **4. What We're Going to Do**
- **Next Steps:** List immediate next actions to be taken
- **Pending Tasks:** Document tasks that are planned but not yet started
- **Dependencies:** Note any dependencies that need to be resolved
- **Priorities:** Rank upcoming tasks by priority or importance

---

## **Output Format**

Create a context file with the following structure:

```markdown
# Session Context - [CHAT TITLE]

## Session Summary
[Brief 1-2 sentence summary of the session's main focus]

## What We've Done
### Completed Tasks
- [ ] Task 1: Description
- [ ] Task 2: Description

### Files Modified
- `path/to/file1.tsx` - Created/Modified/Deleted
- `path/to/file2.ts` - Created/Modified/Deleted

### Key Decisions
- Decision 1: Rationale
- Decision 2: Rationale

### Progress Metrics
- Migration progress: X%
- Components converted: X/Y
- Tests passing: X/Y

## What Worked
### Successful Patterns
- Pattern 1: Description and why it worked
- Pattern 2: Description and why it worked

### Effective Tools
- Tool 1: How it helped
- Tool 2: How it helped

### Problem Solutions
- Problem 1: Solution and outcome
- Problem 2: Solution and outcome

## What Didn't Work
### Failed Approaches
- Approach 1: Why it failed and what was learned
- Approach 2: Why it failed and what was learned

### Error Patterns
- Error 1: Description and frequency
- Error 2: Description and frequency

### Blockers
- Blocker 1: Description and impact
- Blocker 2: Description and impact

## What We're Going to Do
### Immediate Next Steps
1. [ ] Action 1: Description and expected outcome
2. [ ] Action 2: Description and expected outcome

### Pending Tasks
- [ ] Task 1: Description and priority
- [ ] Task 2: Description and priority

### Dependencies
- Dependency 1: What's needed and timeline
- Dependency 2: What's needed and timeline

### Current Focus
[Description of the main area of focus for the next session]

## Session Metadata
- **Duration:** [Session length]
- **Primary Commands Used:** [List of main commands/tools used]
- **Key Files:** [List of most important files worked with]
- **Context Tags:** [Tags for easy searching: migration, debug, feature, etc.]
```

---

## **File Naming Convention**

Save the context file to: `C:\dev\cursor\saved-context`

**Filename:** Use the current chat window title, converted to lowercase with spaces replaced by dashes (-)
- Example: "hxp-frontend-apps Cloud Progress" → `hxp-frontend-apps-cloud-progress.md`
- Example: "Debug API Issues" → `debug-api-issues.md`
- Example: "Component Migration" → `component-migration.md`

**Full path format:** `C:\dev\cursor\saved-context\[chat-title].md`

---

## **Usage Examples**
- `/save-context` (at any point during development)
- Use before ending a development session
- Use before switching to a different task or project
- Use after completing significant milestones
- Use when encountering complex problems that need continued attention

---

## **Integration with Other Commands**

This command complements:
- `/retro` - For learning capture and doctrine evolution
- `/analyze` - For project state analysis
- `/do` - For task execution tracking

**Note:** Unlike `/retro` which focuses on learning and doctrine evolution, `/save-context` focuses on preserving work state and continuity for future sessions.