# Root Cause Analysis & Deep Diagnostic Protocol

## Overview
For persistent bugs where standard procedures have failed. Initiates systematic root cause analysis with evidence-based remediation. Use when simpler attempts haven't worked.

## Mission Briefing: Root Cause Analysis & Remediation Protocol

**ANALYSIS COMMAND DETECTION:** Before proceeding with analysis, check if the provided text contains any `/analyze*` command (e.g., `/analyze-general`, `/analyze-code`, `/analyze-project`, `/analyze-bug-*`, etc.). If found, skip this command's analysis and execute the detected command instead to avoid duplicate analysis.

Previous, simpler attempts to resolve this issue have failed. Standard procedures are now suspended. You will initiate a **deep diagnostic protocol.**

Your approach must be systematic, evidence-based, and relentlessly focused on identifying and fixing the **absolute root cause.** Patching symptoms is a critical failure.

---

## **Phase 0: Reconnaissance & State Baseline (Read-Only)**

**Directive:** Adhering to the **Operational Doctrine**, perform a non-destructive scan of the repository, runtime environment, configurations, and recent logs. Your objective is to establish a high-fidelity, evidence-based baseline of the system's current state as it relates to the anomaly.

**Output:** Produce a concise digest (≤ 200 lines) of your findings.

**Constraint:** **No mutations are permitted during this phase.**

---

## **Phase 1: Isolate the Anomaly**

**Directive:** Your first and most critical goal is to create a **minimal, reproducible test case** that reliably and predictably triggers the bug.

**Actions:**
1. **Define Correctness:** Clearly state the expected, non-buggy behavior
2. **Create a Failing Test:** If possible, write a new, specific automated test that fails precisely because of this bug. This test will become your signal for success
3. **Pinpoint the Trigger:** Identify the exact conditions, inputs, or sequence of events that causes the failure

**Constraint:** You will not attempt any fixes until you can reliably reproduce the failure on command.

---

## **Phase 2: Root Cause Analysis (RCA)**

**Directive:** With a reproducible failure, you will now methodically investigate the failing pathway to find the definitive root cause.

**Evidence-Gathering Protocol:**
1. **Formulate a Testable Hypothesis:** State a clear, simple theory about the cause (e.g., "Hypothesis: The user authentication token is expiring prematurely.")
2. **Devise an Experiment:** Design a safe, non-destructive test or observation to gather evidence that will either prove or disprove your hypothesis
3. **Execute and Conclude:** Run the experiment, present the evidence, and state your conclusion. If the hypothesis is wrong, formulate a new one based on the new evidence and repeat this loop

**Anti-Patterns (Forbidden Actions):**
- **FORBIDDEN:** Applying a fix without a confirmed root cause supported by evidence
- **FORBIDDEN:** Re-trying a previously failed fix without new data
- **FORBIDDEN:** Patching a symptom (e.g., adding a `null` check) without understanding *why* the value is becoming `null`

---

## **Phase 3: Findings Presentation & User Approval**

**Directive:** Present comprehensive findings from root cause analysis and wait for user approval before proceeding with remediation.

**Presentation Structure:**

### **Root Cause Analysis Summary**
- **Confirmed Root Cause:** Definitive statement of the underlying issue
- **Supporting Evidence:** Key evidence from RCA that proves the root cause
- **Impact Analysis:** How this root cause affects the system and users
- **Scope of Impact:** Which components and users are affected

### **Proposed Remediation Strategy**
- **Fix Approach:** High-level strategy for addressing the root cause
- **Files to Modify:** List of files that will need changes
- **System-Wide Impact:** Analysis of all consumers that may be affected
- **Risk Assessment:** Potential risks and mitigation strategies
- **Testing Strategy:** How the fix will be verified and validated

### **Implementation Plan**
1. **Phase 1**: [First set of changes]
2. **Phase 2**: [Second set of changes]
3. **Phase 3**: [Third set of changes]
4. **Verification**: [How the fix will be tested]
5. **Rollback Plan**: [How to revert if issues arise]

### **Recommended Next Steps**
1. **Immediate Actions**: [What to do first]
2. **Verification Steps**: [How to confirm the fix works]
3. **Testing Approach**: [How to test the fix thoroughly]
4. **Monitoring**: [What to watch for after implementation]

---

## **Phase 4: User Feedback & Direction**

**Directive:** Present findings and wait for user approval and/or feedback on the proposed remediation approach.

**User Interaction Protocol:**

1. **Present Findings**: Share the complete root cause analysis and proposed remediation
2. **Request Approval**: Ask for user approval to proceed with the proposed fix
3. **Address Concerns**: Answer any questions or concerns about the approach
4. **Refine Strategy**: Adjust the remediation plan based on user feedback
5. **Wait for Direction**: Pause for user approval or further instructions

**Expected User Responses:**
- **Approval**: User agrees with analysis and wants to proceed with fix
- **Modifications**: User wants to modify the proposed approach
- **Additional Analysis**: User wants deeper investigation of specific aspects
- **Different Strategy**: User wants to try a different remediation approach
- **Defer**: User wants to postpone the fix for later consideration

---

## **Phase 5: Remediation (After User Approval)**

**Directive:** Design and implement a minimal, precise fix that durably hardens the system against the confirmed root cause.

**Core Protocols in Effect:**
- **Read-Write-Reread:** For every file you modify, you must read it immediately before and after the change
- **Command Execution Canon:** All shell commands must use the mandated safety wrapper
- **System-Wide Ownership:** If the root cause is in a shared component, you are **MANDATED** to analyze and, if necessary, fix all other consumers affected by the same flaw

---

## **Phase 6: Verification & Regression Guard**

**Directive:** Prove that your fix has resolved the issue without creating new ones.

**Verification Steps:**
1. **Confirm the Fix:** Re-run the specific failing test case from Phase 1. It **MUST** now pass
2. **Run Full Quality Gates:** Execute the entire suite of relevant tests (unit, integration, etc.) and linters to ensure no regressions have been introduced elsewhere
3. **Autonomous Correction:** If your fix introduces any new failures, you will autonomously diagnose and resolve them

---

## **Phase 7: Mandatory Zero-Trust Self-Audit**

**Directive:** Your remediation is complete, but your work is **NOT DONE.** You will now conduct a skeptical, zero-trust audit of your own fix.

**Audit Protocol:**
1. **Re-verify Final State:** With fresh commands, confirm that all modified files are correct and that all relevant services are in a healthy state
2. **Hunt for Regressions:** Explicitly test the primary workflow of the component you fixed to ensure its overall functionality remains intact

---

## **Phase 8: Final Report & Verdict**

**Directive:** Conclude your mission with a structured "After-Action Report."

**Report Structure:**
- **Root Cause:** A definitive statement of the underlying issue, supported by the key piece of evidence from your RCA
- **Remediation:** A list of all changes applied to fix the issue
- **Verification Evidence:** Proof that the original bug is fixed (e.g., the passing test output) and that no new regressions were introduced (e.g., the output of the full test suite)
- **Final Verdict:** Conclude with one of the two following statements, exactly as written:
  - `"Self-Audit Complete. Root cause has been addressed, and system state is verified. No regressions identified. Mission accomplished."`
  - `"Self-Audit Complete. CRITICAL ISSUE FOUND during audit. Halting work. [Describe issue and recommend immediate diagnostic steps]."`

**Constraint:** Maintain an inline TODO ledger using ✅ / ⚠️ / 🚧 markers throughout the process.

## Usage Examples
- `/refresh The authentication keeps failing intermittently`
- `/refresh Users report data loss after form submission`
- `/refresh Build process randomly fails with memory errors`
