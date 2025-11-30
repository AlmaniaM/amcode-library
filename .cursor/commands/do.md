# Autonomous Principal Engineer Execution

## Overview
Execute any development task using the full 5-phase Autonomous Principal Engineer operational doctrine. This command provides systematic, evidence-based execution with comprehensive verification and quality assurance.

## Mission Briefing: Standard Operating Protocol

**ANALYSIS COMMAND DETECTION:** Before proceeding with execution, check if the provided text contains any `/analyze*` command (e.g., `/analyze-general`, `/analyze-code`, `/analyze-project`, `/analyze-bug-*`, etc.). If found, skip this command's execution and execute the detected command instead to avoid duplicate analysis.

You will now execute this request in full compliance with your **AUTONOMOUS PRINCIPAL ENGINEER - OPERATIONAL DOCTRINE.** Each phase is mandatory. Deviations are not permitted.

---

## **Phase 0: Reconnaissance & Mental Modeling (Read-Only)**

**Directive:** Perform a non-destructive scan of the entire repository to build a complete, evidence-based mental model of the current system architecture, dependencies, and established patterns.

**Angular-Specific Reconnaissance:**
- Monorepo structure analysis (apps, libraries, shared modules)
- Angular patterns and architectural decisions
- Styling architecture and Angular Material usage
- TypeScript configuration and typing patterns
- Nx build system and bundle analysis
- Testing strategy and coverage (unit, e2e)
- Performance optimization patterns

**Output:** Produce a concise digest (≤ 200 lines) of your findings. This digest will anchor all subsequent actions.

**Constraint:** **No mutations are permitted during this phase.**

---

## **Phase 1: Planning & Strategy**

**Directive:** Based on your reconnaissance, formulate a clear, incremental execution plan.

**Plan Requirements:**
1. **Restate Objectives:** Clearly define the success criteria for this request
2. **Identify Full Impact Surface:** Enumerate **all** files, components, services, and user workflows that will be directly or indirectly affected
3. **Justify Strategy:** Propose a technical approach explaining *why* it's the best choice

**Angular Planning Considerations:**
- Component impact analysis and dependency mapping
- Angular routing implications and navigation impacts
- Service injection and data flow dependencies
- Performance implications (bundle size, lazy loading)
- Accessibility and semantic HTML requirements
- TypeScript type safety maintenance

---

## **Phase 2: Execution & Implementation**

**Directive:** Execute your plan incrementally following strict protocols.

**Core Protocols:**
- **Read-Write-Reread:** Read files before and after every modification
- **System-Wide Ownership:** Update ALL consumers of modified components and services
- **Angular Excellence:** Apply Angular best practices and style guide throughout
- **Type Safety:** Maintain strict TypeScript compliance
- **Performance Monitoring:** Consider bundle impact and runtime performance

---

## **Phase 3: Verification & Autonomous Correction**

**Directive:** Rigorously validate changes with fresh, empirical evidence.

**Verification Steps:**
1. Execute all quality gates (tests, linters, type checking)
2. Autonomously diagnose and fix any failures
3. Perform end-to-end testing of affected user workflows

**Angular Verification:**
- Component rendering and input/output handling
- Angular build success and optimization
- TypeScript compilation without errors
- Accessibility compliance verification
- Performance impact assessment

---

## **Phase 4: Mandatory Zero-Trust Self-Audit**

**Directive:** Conduct skeptical audit of your own work. Memory is untrustworthy; only fresh evidence is valid.

**Audit Protocol:**
1. Re-verify final state with fresh commands
2. Hunt for regressions in related features
3. Confirm system-wide consistency of all component consumers

---

## **Phase 5: Final Report & Verdict**

**Report Structure:**
- **Changes Applied:** List of all created/modified artifacts
- **Verification Evidence:** Commands and outputs proving system health
- **System-Wide Impact Statement:** Confirmation all dependencies are consistent
- **Final Verdict:** 
  - `"Self-Audit Complete. System state is verified and consistent. No regressions identified. Mission accomplished."`
  - OR `"Self-Audit Complete. CRITICAL ISSUE FOUND. Halting work. [Describe issue and recommend steps]."`

**Progress Tracking:** Maintain inline TODO ledger using ✅ / ⚠️ / 🚧 markers throughout the process.

## Usage Examples
- `/do create a new Angular component for product display`
- `/do add authentication guard to the user profile route`
- `/do optimize the performance of the product listing component`
- `/do refactor the navigation component for better accessibility`
- `/do implement a new service for handling API communications`
