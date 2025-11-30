# Create Page From Figma Command

## Purpose
Create a comprehensive implementation plan for React Native page components from Figma designs (mobile and desktop). This command analyzes Figma designs, extracts design tokens, creates a structured plan following the create-themed-component pattern, and ensures all gradients are added to the theme system for reusability. Supports composition of components from multiple Figma designs with natural language instructions specifying which components to extract from which designs.

## When to Use
- **New Page Implementation**: When creating new pages/screens from Figma designs
- **Design-to-Code Conversion**: When converting Figma designs to React Native components
- **Multi-Platform Pages**: When implementing pages that have both mobile and desktop variants
- **Multi-Source Composition**: When combining components from multiple Figma designs into a single page
- **Component Extraction**: When extracting specific components (filters, sidebars, headers) from different design screens
- **Theme Integration**: When ensuring new pages follow the project's theming system
- **Design System Compliance**: When ensuring pages match the project's design system

## Command Structure

### Phase 1: Figma Design Analysis
**AUTONOMOUS PRINCIPAL ENGINEER DOCTRINE**: Begin with comprehensive understanding of the Figma designs.

#### Component Extraction Instruction Parsing
- [ ] **REQUIRED**: Parse natural language instructions to identify:
  - Mobile component extraction instructions (e.g., "take the filter settings component from this page")
  - Desktop component extraction instructions (e.g., "take the side nav from this screen")
  - Component names/descriptions mentioned in instructions
  - Figma URLs associated with each component extraction
- [ ] **REQUIRED**: Map component names to their source Figma URLs
  - Create component-to-source mapping for mobile components
  - Create component-to-source mapping for desktop components
  - Identify "rest of components" or default source for remaining elements
- [ ] **REQUIRED**: Validate instruction clarity
  - Ensure all components have clear source assignments
  - Identify ambiguous instructions and request clarification if needed
  - Document component extraction mapping

#### Figma URL Processing
- [ ] **REQUIRED**: Extract fileKey and nodeId from all mobile Figma URLs
  - Parse each mobile URL mentioned in instructions
  - Extract fileKey from URL path (e.g., `W1S2YUiEix1LajKOVVe1Gz`)
  - Extract nodeId from query parameter (e.g., `11609-21961` → convert to `11609:21961`)
- [ ] **REQUIRED**: Extract fileKey and nodeId from all desktop Figma URLs (if provided)
  - Parse each desktop URL mentioned in instructions
  - Extract fileKey and nodeId for each desktop source
- [ ] **REQUIRED**: Validate all Figma URLs are accessible
  - Test each URL for accessibility
  - Verify nodeIds are valid
- [ ] **REQUIRED**: Fetch Figma design context for all mobile designs
  - Fetch design context for each mobile source URL
  - Store design context with component mapping
- [ ] **REQUIRED**: Fetch Figma design context for all desktop designs (if provided)
  - Fetch design context for each desktop source URL
  - Store design context with component mapping
- [ ] **REQUIRED**: Fetch screenshots for visual reference
  - Get screenshots for each source design
  - Organize screenshots by component source

#### Design Element Extraction
- [ ] **REQUIRED**: Extract components from mobile sources according to instructions
  - For each specified component (e.g., "filter settings component"):
    - Locate component in the designated source design
    - Extract all UI elements within that component
    - Document component boundaries and structure
  - For "rest of components" or default source:
    - Extract all remaining UI elements not assigned to specific sources
    - Headers, titles, descriptions
    - Form fields, inputs, labels
    - Buttons (primary, secondary, social)
    - Checkboxes, radio buttons, toggles
    - Dividers, separators
    - Icons, images, logos
    - Footer elements, links
    - **Bottom navigation bars**: Identify if design includes bottom tab navigation or bottom action bar
      - Check if page is a detail/edit screen that should have back navigation
      - Determine if back button should be added to bottom nav bar
      - Document bottom nav bar structure and required buttons
    - **Top navigation bars/toolbars**: Identify if design includes top navbar
      - **CRITICAL for Mobile**: Only use top navbar if explicitly shown in design or user specifies
      - For mobile, prefer alternatives: side nav, bottom nav, in-screen elements
      - For desktop, top navbar/toolbar is preferred and acceptable
      - Document whether top navbar is explicitly required or can be replaced with alternatives
- [ ] **REQUIRED**: Extract components from desktop sources according to instructions (if provided)
  - For each specified component (e.g., "side nav", "filters"):
    - Locate component in the designated source design
    - Extract all UI elements within that component
    - Document component boundaries and structure
  - For "rest of components" or default source:
    - Extract all remaining UI elements not assigned to specific sources
    - Note differences from mobile (layout, spacing, elements)
    - Identify desktop-specific features
- [ ] **REQUIRED**: Compose final component structure
  - Combine extracted components into final mobile page structure
  - Combine extracted components into final desktop page structure (if provided)
  - Document component composition and layout relationships
  - Identify any missing components or gaps
- [ ] **REQUIRED**: Extract design tokens from all source designs
  - Colors (hex values, gradients) from all sources
  - Spacing (padding, margins, gaps) from all sources
  - Border radius values from all sources
  - Typography (font sizes, weights, line heights) from all sources
  - Component dimensions (heights, widths) from all sources
  - Document which tokens come from which source

#### Gradient Analysis
- [ ] **REQUIRED**: Identify all gradients in all source designs
  - Extract gradient colors (start, end) from each source
  - Extract gradient direction (horizontal, vertical, diagonal) from each source
  - Extract gradient opacity values from each source
  - Document which gradients come from which source design
- [ ] **REQUIRED**: Consolidate gradients across sources
  - Identify duplicate gradients across different sources
  - Merge gradient information from multiple sources
  - Create unified gradient list for theme integration
- [ ] **REQUIRED**: Check existing theme system for gradient definitions
  - File: `recipe-ocr-mobile-app/src/theme/types.ts`
  - Check `theme.colors.primaryGradient` or similar properties
  - Check `theme.gradients` functions
- [ ] **REQUIRED**: Determine if gradients need to be added to theme
  - Compare extracted gradients with existing theme gradients
  - Identify unique gradients that don't exist
  - Plan gradient additions to theme system
  - Document gradient source mapping for reference

### Phase 2: Component Structure Planning
**Following create-themed-component pattern**: Plan component structure for mobile and desktop variants.

#### Component Architecture
- [ ] **REQUIRED**: Determine component name from design (e.g., SignUpForm, LoginForm)
- [ ] **REQUIRED**: Plan folder structure:
  - `recipe-ocr-mobile-app/src/components/[category]/[ComponentName]/`
  - `[ComponentName]Mobile.tsx` - Mobile component
  - `[ComponentName]Desktop.tsx` - Desktop component (if desktop design provided)
  - `[ComponentName].tsx` - Platform-adaptive wrapper
  - `Themed[ComponentName]Mobile.tsx` - Themed mobile wrapper
  - `Themed[ComponentName]Desktop.tsx` - Themed desktop wrapper
  - `Themed[ComponentName].tsx` - Themed platform wrapper
  - `index.ts` - Barrel exports
- [ ] **REQUIRED**: Plan component props interface
  - Extract required props from design interactions
  - Plan callback props (onSubmit, onPress, etc.)
  - Plan state props (isLoading, errors, etc.)
  - Plan optional props (onBackPress, onSocialLogin, etc.)
  - **Bottom Navigation Props**: If bottom nav bar is present:
    - Plan `bottomTabButtons` prop for mobile (array of button configs)
    - Plan `sidebarActions` prop for desktop (array of action configs)
    - Include `onBackPress` callback if back button is needed
    - Document button structure: `{ key, label, icon, onPress, variant }`

#### Platform Detection Strategy
- [ ] **REQUIRED**: Plan platform detection logic
  - Use `Dimensions` API for screen width detection
  - Use breakpoint: 768px (mobile ≤768px, desktop >768px)
  - Plan responsive behavior
- [ ] **REQUIRED**: Document platform-specific differences
  - Layout differences
  - Element differences (e.g., back button on mobile, logo on desktop)
  - Spacing/sizing differences
- [ ] **REQUIRED**: Plan navigation structure by platform
  - **Mobile (≤768px)**: 
    - **AVOID top navbar/toolbar unless explicitly specified in design**
    - Prefer alternatives: side nav (when applicable), bottom nav bar, in-screen elements
    - Use top navbar only if design explicitly shows it or user specifies it
    - Document navigation placement decision and rationale
  - **Desktop (>768px)**:
    - Top navbar/toolbar is **preferred and acceptable**
    - Can use top toolbar for actions and navigation
    - Side nav is also acceptable for desktop
    - Document navigation placement decision

### Phase 3: Theme System Integration Planning
**Theme System Enhancement**: Plan additions to theme system for gradients and missing tokens.

#### Gradient Theme Integration
- [ ] **REQUIRED**: Plan gradient additions to theme types
  - File: `recipe-ocr-mobile-app/src/theme/types.ts`
  - Add `primaryGradient: { start: string; end: string }` to colors interface (if not exists)
  - Add other gradient properties if needed
- [ ] **REQUIRED**: Plan gradient utility enhancements
  - File: `recipe-ocr-mobile-app/src/theme/utils.ts`
  - Enhance `createGradientFunction` for native support with expo-linear-gradient
  - Create `createNativeGradient` helper if needed
- [ ] **REQUIRED**: Plan gradient additions to all theme files
  - Files: All `*.theme.ts` files
  - Add `primaryGradient` to each theme
  - Use primary color as base, create gradient variant
  - **CRITICAL**: Check if gradient already exists before adding (avoid duplicates)
  - If gradient exists with same colors, reuse existing gradient property

#### Design Token Mapping
- [ ] **REQUIRED**: Map Figma colors to theme colors
  - Map hex colors to `theme.colors.*` properties
  - Map opacity values to `theme.opacity.*` properties
  - Document color mapping decisions
- [ ] **REQUIRED**: Map Figma spacing to theme spacing
  - Map pixel values to `theme.spacing.*` properties
  - Verify spacing follows design system scale
- [ ] **REQUIRED**: Map Figma border radius to theme borderRadius
  - Map values to `theme.borderRadius.*` properties
  - Reference design system specifications
- [ ] **REQUIRED**: Map Figma typography to ThemedText variants
  - Map font sizes/weights to ThemedText variants
  - Plan ThemedTextInput usage for inputs

### Phase 4: Implementation Plan Creation
**Structured Planning**: Create comprehensive implementation plan following save-plan format.

#### Planning Document Structure
- [ ] **REQUIRED**: Create planning document
  - Location: `planning/front-end/design/designed-pages-work-in-progress/[component-name].plan.md`
  - Follow save-plan.md structure
- [ ] **REQUIRED**: Document Progress Tracking
  - Status: Planning Complete
  - Current Phase: Phase 0 - Planning & Analysis
  - Implementation Progress table with all phases
  - Active Work section
  - Detailed task breakdown
- [ ] **REQUIRED**: Document Implementation Strategy
  - High-level approach
  - Key architecture decisions
  - Patterns to follow (create-themed-component pattern)
  - Integration points
- [ ] **REQUIRED**: Document Technical Context
  - Files to create/modify
  - Dependencies
  - Code patterns
- [ ] **REQUIRED**: Document Design Reference
  - Component Source Mapping:
    - Mobile component sources: List each component and its source Figma URL/Node ID
    - Desktop component sources: List each component and its source Figma URL/Node ID
    - Component extraction instructions summary
  - All Figma URLs and Node IDs (organized by component)
  - Design elements for both platforms (with source attribution)
  - Design tokens (for reference only - note they'll use theme values)
  - Component composition structure showing how components combine

#### Phase Breakdown
- [ ] **REQUIRED**: Phase 1: Theme System Enhancement
  - Add gradient support to theme
  - Update gradient utilities
  - Add gradients to all theme files (check for duplicates)
- [ ] **REQUIRED**: Phase 2: Component Structure Creation
  - Create folder structure
  - Define props interface
  - Create platform-specific component files
  - Create index.ts exports
- [ ] **REQUIRED**: Phase 3: Base Component Implementation
  - Mobile component tasks (header, form fields, buttons, etc.)
  - Desktop component tasks (if desktop design provided)
  - Platform-adaptive wrapper
  - **Navigation Structure Implementation**:
    - **Mobile Navigation**:
      - **AVOID top navbar unless explicitly specified in design**
      - If top navbar is not explicitly required, use alternatives:
        - Side nav for navigation items (when applicable)
        - Bottom nav bar for actions and navigation
        - In-screen elements (buttons, action bars) for better mobile UX
      - Only implement top navbar if design explicitly shows it or user specifies it
      - Document navigation placement decision and UX rationale
    - **Desktop Navigation**:
      - Top navbar/toolbar is **preferred and acceptable**
      - Can use top toolbar for actions, navigation, and controls
      - Side nav is also acceptable for desktop layouts
      - Document navigation placement decision
  - **Bottom Navigation Integration** (if applicable):
    - Add `BottomTabButtons` component for mobile bottom nav bar
    - Add `ActionSidebar` component for desktop action sidebar
    - **CRITICAL**: Always include back button in bottom nav bar for detail/edit screens
    - Implement back button with proper navigation handler (`router.back()` or `onBackPress`)
    - Style back button according to design system (primary or secondary variant)
    - Ensure back button is first in button array for mobile bottom nav
- [ ] **REQUIRED**: Phase 4: Themed Component Implementation
  - Themed mobile wrapper
  - Themed desktop wrapper (if desktop design provided)
  - Apply theme-based styles
- [ ] **REQUIRED**: Phase 5: Gradient Implementation
  - Create gradient helper components if needed
  - Integrate gradients with theme
- [ ] **REQUIRED**: Phase 6: Icon Integration
  - Add required icons
  - Style icons with theme
- [ ] **REQUIRED**: Phase 7: Responsive Platform Component (if desktop provided)
  - Create platform-adaptive component
  - Create themed platform wrapper
- [ ] **REQUIRED**: Phase 8: Verification & Testing
  - Theme switching verification
  - Component functionality testing
  - Code quality verification

### Phase 5: Planning Document Output
**Comprehensive Documentation**: Create detailed planning document ready for implementation.

#### Document Sections
- [ ] **REQUIRED**: Progress Tracking section
  - Implementation Progress table
  - Active Work details
  - What's Left breakdown with all tasks
- [ ] **REQUIRED**: Implementation Strategy section
  - Architecture decisions
  - Patterns to follow
  - Integration points
- [ ] **REQUIRED**: Technical Context section
  - Files involved (primary, supporting, reference)
  - Dependencies
  - Code patterns with examples
- [ ] **REQUIRED**: Design Reference section
  - Component Source Mapping:
    - Mobile: Component name → Source Figma URL/Node ID
    - Desktop: Component name → Source Figma URL/Node ID
    - Extraction instructions summary
  - All Figma URLs and Node IDs (organized by component)
  - Mobile design elements (with source attribution)
  - Desktop design elements (if provided, with source attribution)
  - Component composition diagram/structure
  - Design tokens (reference only, with source attribution)
- [ ] **REQUIRED**: Handoff Checklist
  - All items checked
  - Clear next steps for implementation

## Usage

### Command Format

#### Simple Format (Single Source Per Platform)
```
/create-page-from-figma [mobile-figma-url] [desktop-figma-url]
```

#### Advanced Format (Multiple Sources with Component Extraction)
```
/create-page-from-figma [natural-language-instructions-with-figma-urls]
```

### Parameters

#### Simple Format Parameters
- **mobile-figma-url** (REQUIRED): Full Figma URL for mobile design
  - Format: `https://www.figma.com/design/[fileKey]/[fileName]?node-id=[nodeId]`
  - Example: `https://www.figma.com/design/W1S2YUiEix1LajKOVVe1Gz/gluestack-ui-v2.0-Design-Kit--Community-?node-id=11609-21961&t=uHZojGRnFEwBnlF5-4`
- **desktop-figma-url** (OPTIONAL): Full Figma URL for desktop design
  - Format: Same as mobile URL
  - If not provided, only mobile component will be planned

#### Advanced Format Instructions
- **Natural Language Instructions**: Describe which components to extract from which Figma URLs
  - Format: Natural language with Figma URLs embedded
  - Component extraction keywords: "take [component-name] from", "extract [component-name] from", "use [component-name] from"
  - Default/rest keywords: "rest of components", "rest from", "remaining components"
  - Platform keywords: "mobile", "desktop", "for mobile", "for desktop"

### Example Usage

#### Simple Example (Single Source)
```
/create-page-from-figma https://www.figma.com/design/W1S2YUiEix1LajKOVVe1Gz/gluestack-ui-v2.0-Design-Kit--Community-?node-id=11609-21961&t=uHZojGRnFEwBnlF5-4 https://www.figma.com/design/W1S2YUiEix1LajKOVVe1Gz/gluestack-ui-v2.0-Design-Kit--Community-?node-id=11671-23563&t=uHZojGRnFEwBnlF5-4
```

#### Advanced Example (Multiple Sources with Component Extraction)
```
/create-page-from-figma take the filter settings component from this page https://www.figma.com/design/W1S2YUiEix1LajKOVVe1Gz/Design-Kit?node-id=11609-21961 and the rest of the components from this page https://www.figma.com/design/W1S2YUiEix1LajKOVVe1Gz/Design-Kit?node-id=11671-23563. Take the side nav from this screen https://www.figma.com/design/W1S2YUiEix1LajKOVVe1Gz/Design-Kit?node-id=11700-24000, the filters from this page https://www.figma.com/design/W1S2YUiEix1LajKOVVe1Gz/Design-Kit?node-id=11710-24500 and the rest from this screen https://www.figma.com/design/W1S2YUiEix1LajKOVVe1Gz/Design-Kit?node-id=11720-25000
```

#### Advanced Example (Mobile Only with Multiple Sources)
```
/create-page-from-figma for mobile: take the filter settings component from https://www.figma.com/design/W1S2YUiEix1LajKOVVe1Gz/Design-Kit?node-id=11609-21961 and the rest of the components from https://www.figma.com/design/W1S2YUiEix1LajKOVVe1Gz/Design-Kit?node-id=11671-23563
```

#### Advanced Example (Desktop Only with Multiple Sources)
```
/create-page-from-figma for desktop: take the side nav from https://www.figma.com/design/W1S2YUiEix1LajKOVVe1Gz/Design-Kit?node-id=11700-24000, the filters from https://www.figma.com/design/W1S2YUiEix1LajKOVVe1Gz/Design-Kit?node-id=11710-24500 and the rest from https://www.figma.com/design/W1S2YUiEix1LajKOVVe1Gz/Design-Kit?node-id=11720-25000
```

## Implementation Workflow

### Step 1: Parse Component Extraction Instructions
1. **If simple format** (two URLs provided):
   - Treat first URL as mobile source
   - Treat second URL as desktop source (if provided)
   - All components come from respective single source
2. **If advanced format** (natural language instructions):
   - Parse natural language to identify:
     - Mobile component extraction instructions
     - Desktop component extraction instructions
     - Component names and their source URLs
     - "Rest of components" or default source assignments
   - Create component-to-source mapping:
     - Mobile: `{ componentName: { url, nodeId, fileKey } }`
     - Desktop: `{ componentName: { url, nodeId, fileKey } }`
   - Validate all components have source assignments
   - Request clarification for ambiguous instructions

### Step 2: Extract Figma Information
1. **For each unique Figma URL identified**:
   - Parse URL to extract:
     - `fileKey`: From URL path (e.g., `W1S2YUiEix1LajKOVVe1Gz`)
     - `nodeId`: From query parameter (e.g., `11609-21961` → convert to `11609:21961`)
   - Use `mcp_Figma_get_design_context` to fetch design data
   - Use `mcp_Figma_get_screenshot` to fetch visual reference
   - Store design context with component mapping

### Step 3: Extract Components from Sources
1. **For mobile components**:
   - For each specified component (e.g., "filter settings"):
     - Locate component in its designated source design
     - Extract component structure and elements
   - For "rest of components":
     - Extract all remaining elements from default source
2. **For desktop components** (if provided):
   - For each specified component (e.g., "side nav", "filters"):
     - Locate component in its designated source design
     - Extract component structure and elements
   - For "rest of components":
     - Extract all remaining elements from default source
3. **Compose final structure**:
   - Combine extracted components into final page structure
   - Document component composition and relationships

### Step 4: Analyze Designs
1. Extract all UI elements from all source designs
2. Identify design tokens (colors, spacing, typography, gradients) from all sources
3. Consolidate design tokens across sources
4. Compare with existing theme system
5. Identify missing theme properties
6. Check for existing gradients to avoid duplicates
7. Document token source attribution

### Step 5: Plan Component Structure
1. Determine component name from composed design
2. Plan folder structure following create-themed-component pattern
3. Plan props interface based on design interactions
4. Plan platform detection if desktop design provided
5. Document component composition structure
6. **Plan Navigation Structure**:
   - **For Mobile (≤768px)**:
     - Check if design explicitly shows top navbar/toolbar
     - If top navbar is NOT explicitly shown, plan alternatives:
       - Side nav for navigation items (when applicable)
       - Bottom nav bar for actions and navigation
       - In-screen elements for better mobile UX
     - Only plan top navbar if design explicitly shows it or user specifies it
     - Document navigation placement decision and UX rationale
   - **For Desktop (>768px)**:
     - Top navbar/toolbar is preferred and acceptable
     - Can plan top toolbar for actions, navigation, controls
     - Side nav is also acceptable
     - Document navigation placement decision
7. **Determine if bottom navigation bar is needed**:
   - Check if design includes bottom tab navigation or action bar
   - Identify if screen is a detail/edit screen (requires back navigation)
   - Plan `bottomTabButtons` prop for mobile if bottom nav is present
   - Plan `sidebarActions` prop for desktop if action sidebar is present
   - **CRITICAL**: Always include back button as first button for detail/edit screens
   - Document navigation flow to confirm back button necessity

### Step 6: Plan Theme Integration
1. Identify gradients that need to be added (from all sources)
2. Check existing theme for duplicate gradients
3. Plan gradient additions (only if not duplicate)
4. Plan other theme property additions if needed
5. Document gradient source mapping

### Step 7: Create Planning Document
1. Create planning document in appropriate location
2. Document all phases and tasks
3. Include design references with component source mapping
4. Document component composition structure
5. Provide clear handoff for next AI session

## Critical Requirements

### Gradient Handling
- **REQUIRED**: Check if gradient already exists in theme before adding
- **REQUIRED**: Compare gradient colors (start and end) with existing gradients
- **REQUIRED**: If gradient exists with same colors, reuse existing gradient property
- **REQUIRED**: Only add new gradients if they don't exist
- **REQUIRED**: Document gradient reuse decisions in planning document

### Theme System Compliance
- **REQUIRED**: All colors must map to `theme.colors.*` properties
- **REQUIRED**: All spacing must map to `theme.spacing.*` properties
- **REQUIRED**: All border radius must map to `theme.borderRadius.*` properties
- **REQUIRED**: All typography must use ThemedText/ThemedTextInput variants
- **REQUIRED**: No hardcoded hex colors, pixel values, or CSS values

### Component Structure Compliance
- **REQUIRED**: Follow create-themed-component folder structure
- **REQUIRED**: Create platform-specific components (Mobile, Desktop)
- **REQUIRED**: Create platform-adaptive wrapper component
- **REQUIRED**: Create themed wrappers for each platform variant
- **REQUIRED**: Create index.ts barrel exports

### Navigation Structure Requirements
- **REQUIRED**: Mobile Navigation (≤768px):
  - **AVOID top navbar/toolbar unless explicitly specified in design or user instruction**
  - Prefer alternative navigation patterns:
    - **Side nav**: Use for navigation items, menu options, filters (when applicable)
    - **Bottom nav bar**: Use for primary actions, tab navigation, action buttons
    - **In-screen elements**: Use buttons, action bars, or floating elements within the screen content
  - Only use top navbar if:
    - Design explicitly shows a top navbar/toolbar
    - User specifically requests top navbar
    - No suitable alternative exists and top navbar provides better UX
  - Document navigation placement decision and rationale in planning document
- **REQUIRED**: Desktop Navigation (>768px):
  - Top navbar/toolbar is **preferred and acceptable**
  - Can use top toolbar for actions, navigation controls, breadcrumbs
  - Side nav is also acceptable for desktop layouts
  - Document navigation placement decision

### Bottom Navigation Bar Requirements
- **REQUIRED**: For detail/edit screens with bottom navigation bars:
  - **ALWAYS** include a back button as the first button in the bottom nav bar
  - Back button should use `arrow-back` icon from MaterialIcons
  - Back button label should be "Back" or match design specification
  - Back button should call `router.back()` or provided `onBackPress` handler
  - Back button variant should be `primary` or `secondary` based on design
- **REQUIRED**: For screens that are not root/main screens:
  - Check if screen is accessed via navigation (not a root tab)
  - If yes, include back button in bottom nav bar
  - Document navigation flow to determine if back button is needed
- **REQUIRED**: Bottom nav bar button structure:
  - Mobile: Use `BottomTabButtons` component with `bottomTabButtons` prop
  - Desktop: Use `ActionSidebar` component with `sidebarActions` prop
  - Button config: `{ key: 'back', label: 'Back', icon: 'arrow-back', onPress: handleBack, variant: 'primary' }`

### Planning Document Quality
- **REQUIRED**: Follow save-plan.md structure exactly
- **REQUIRED**: Include all required sections
- **REQUIRED**: Provide detailed task breakdowns
- **REQUIRED**: Include design references with URLs
- **REQUIRED**: Provide clear next steps for implementation

## Success Criteria

### Planning Document Quality
- [ ] All required sections present and complete
- [ ] All tasks broken down into 10-20 minute chunks
- [ ] Design references included with URLs (and component source mapping for multi-source designs)
- [ ] Component source mapping documented (component name → Figma URL/Node ID)
- [ ] Component composition structure documented
- [ ] Theme integration clearly planned
- [ ] Gradient handling documented (duplicate check included, with source attribution)
- [ ] Platform differences clearly documented
- [ ] Clear handoff for next AI session

### Design Analysis Quality
- [ ] All UI elements identified
- [ ] All design tokens extracted (with source attribution)
- [ ] All gradients identified and checked for duplicates (with source attribution)
- [ ] Theme mapping decisions documented
- [ ] Platform differences identified
- [ ] Component source mapping documented (for multi-source designs)
- [ ] Component composition structure validated

### Component Planning Quality
- [ ] Component structure follows create-themed-component pattern
- [ ] Props interface comprehensive
- [ ] Platform detection strategy clear
- [ ] Theme integration planned
- [ ] All files to create/modify identified
- [ ] Navigation structure planned (top nav vs alternatives)
- [ ] Mobile navigation avoids top navbar unless explicitly specified
- [ ] Mobile navigation uses alternatives (side nav, bottom nav, in-screen) when appropriate
- [ ] Desktop navigation uses top navbar/toolbar (preferred) or side nav
- [ ] Navigation placement decision documented with UX rationale
- [ ] Bottom navigation bar planned (if applicable)
- [ ] Back button included in bottom nav for detail/edit screens (if applicable)
- [ ] Navigation flow documented to determine back button necessity

## Integration with Other Commands

### Pre-Planning
- Use with `/understand` to understand project structure before planning
- Use with `/analyze-code-deep` to understand existing component patterns

### Post-Planning
- Use with `/save-plan` to update planning document during implementation
- Use with `/create-themed-component` for component implementation
- Use with `/implement` to execute the plan

## Notes

- **Gradient Duplicate Check**: Always check existing theme gradients before adding new ones. Compare both start and end colors. If a gradient with the same colors exists, reuse it rather than creating a duplicate.
- **Platform Detection**: Use 768px as the breakpoint for mobile vs desktop. Mobile: ≤768px, Desktop: >768px.
- **Theme Values Only**: All design tokens from Figma are for reference only. The actual implementation must use theme system values exclusively.
- **Component Naming**: Derive component name from design (e.g., "Create Account" → `SignUpForm`, "Login" → `LoginForm`).
- **Multi-Source Component Extraction**: When using advanced format with multiple sources:
  - Parse natural language instructions carefully to identify component names and their sources
  - Ensure every component has a clear source assignment
  - Document component source mapping in planning document for reference
  - Handle "rest of components" as default source for remaining elements
  - Validate component composition makes sense (no missing pieces, logical layout)
- **Component Extraction Keywords**: Common phrases to recognize:
  - "take [component] from" / "extract [component] from" / "use [component] from"
  - "rest of components" / "rest from" / "remaining components"
  - "for mobile" / "for desktop" / "mobile:" / "desktop:"
- **Component Source Attribution**: Always document which components come from which Figma designs in the planning document for traceability and future reference.
- **Mobile Navigation Pattern**: 
  - **AVOID top navbar/toolbar on mobile unless explicitly specified**
  - Mobile screens should prefer alternative navigation:
    - **Side nav**: For navigation items, menu options, filters (when applicable)
    - **Bottom nav bar**: For primary actions, tab navigation, action buttons
    - **In-screen elements**: Buttons, action bars, floating elements within content
  - Only use top navbar on mobile if:
    - Design explicitly shows a top navbar/toolbar
    - User specifically requests top navbar
    - No suitable alternative exists and top navbar provides better UX
  - Desktop can use top navbar/toolbar (preferred and acceptable)
  - Document navigation placement decision and rationale in planning document
- **Bottom Navigation Back Button**: 
  - **ALWAYS** add a back button to bottom navigation bars for detail/edit screens
  - Back button should be the first button in the array
  - Use `arrow-back` icon, "Back" label, and `router.back()` or `onBackPress` handler
  - Apply to screens that are accessed via navigation (not root/main screens)
  - Example: Recipe detail screen, edit screens, settings sub-screens
  - Exception: Root/main screens (list screens, home screens) do not need back button
  - Document in planning document whether back button is needed based on navigation flow

**Remember**: This command creates a comprehensive planning document that enables systematic implementation of Figma designs as React Native components following the project's theming system and component patterns. The advanced format supports composing pages from multiple Figma designs by extracting specific components from different sources. Always avoid top navbar on mobile unless explicitly specified - prefer side nav, bottom nav, or in-screen elements for better mobile UX. Desktop can use top navbar/toolbar as preferred. Always include back button in bottom nav bars for detail/edit screens where applicable.

