# Performance Analysis Protocol

## Overview
Execute comprehensive performance analysis of Angular applications, focusing on bundle size, runtime performance, Core Web Vitals, and optimization opportunities.

## Mission Briefing: Performance Analysis Protocol

**ANALYSIS COMMAND DETECTION:** Before proceeding with analysis, check if the provided text contains any `/analyze*` command (e.g., `/analyze-code`, `/analyze-project`, `/analyze-bug-*`, etc.). If found, skip this command's analysis and execute the detected command instead to avoid duplicate analysis.

You will now execute comprehensive performance analysis using the **AUTONOMOUS PRINCIPAL ENGINEER - OPERATIONAL DOCTRINE.** This analysis follows the Phase 0 reconnaissance principles with specialized focus on performance characteristics and optimization opportunities.

---

## **Phase 0: Performance Reconnaissance & Mental Modeling (Read-Only)**

**Directive:** Perform non-destructive analysis of performance characteristics to build a complete understanding of current performance state.

**Performance Analysis Scope:**
- **Bundle Analysis**: Bundle size, composition, and optimization opportunities
- **Runtime Performance**: Component performance, change detection, memory usage
- **Core Web Vitals**: LCP, FID, CLS, FCP, TTI measurements
- **Network Performance**: API calls, data fetching, caching strategies
- **Asset Optimization**: Images, fonts, CSS, JavaScript optimization
- **Angular-Specific**: Change detection, lazy loading, service performance

**Constraints:**
- **No mutations are permitted during this phase**
- **Focus solely on understanding current performance characteristics**
- **Identify optimization opportunities without implementing changes**

---

## **Phase 1: Bundle Analysis**

**Directive:** Analyze bundle composition, size, and optimization opportunities.

**Bundle Analysis Areas:**

1. **Bundle Size Analysis:**
   - Total bundle size and gzipped size
   - Initial bundle vs lazy-loaded chunks
   - Vendor bundle composition
   - Library bundle impact

2. **Bundle Composition Analysis:**
   - Largest modules and their impact
   - Unused code and dead code elimination
   - Duplicate dependencies
   - Tree shaking effectiveness

3. **Code Splitting Analysis:**
   - Route-based code splitting implementation
   - Lazy loading effectiveness
   - Chunk optimization opportunities
   - Dynamic import usage

4. **Dependency Analysis:**
   - Heavy dependencies and alternatives
   - Version compatibility and optimization
   - Peer dependency conflicts
   - Unused dependency removal

**Analysis Commands:**
```bash
# Bundle analysis
npm run build -- --stats-json
npx webpack-bundle-analyzer dist/stats.json

# Bundle size check
npx bundlesize

# Dependency analysis
npm ls --depth=0
npm outdated
```

---

## **Phase 2: Runtime Performance Analysis**

**Directive:** Analyze runtime performance characteristics and bottlenecks.

**Runtime Analysis Areas:**

1. **Component Performance:**
   - Change detection frequency and impact
   - Component rendering performance
   - Memory usage patterns
   - Lifecycle hook performance

2. **Service Performance:**
   - Service instantiation and injection
   - Observable subscription management
   - HTTP request optimization
   - Caching strategy effectiveness

3. **Template Performance:**
   - Template compilation and execution
   - Expression evaluation performance
   - Pipe usage and optimization
   - Structural directive performance

4. **Memory Management:**
   - Memory leak detection
   - Garbage collection patterns
   - Object lifecycle management
   - Subscription cleanup

**Analysis Commands:**
```bash
# Angular DevTools
# Use browser extension for change detection profiling

# Performance profiling
# Use Chrome DevTools Performance tab
# Profile with Angular DevTools

# Memory analysis
# Use Chrome DevTools Memory tab
# Check for memory leaks
```

---

## **Phase 3: Core Web Vitals Analysis**

**Directive:** Analyze Core Web Vitals and user experience metrics.

**Core Web Vitals Analysis:**

1. **Largest Contentful Paint (LCP):**
   - LCP element identification
   - Resource loading optimization
   - Image and font optimization
   - Critical rendering path analysis

2. **First Input Delay (FID):**
   - Main thread blocking analysis
   - JavaScript execution optimization
   - Event handler performance
   - Third-party script impact

3. **Cumulative Layout Shift (CLS):**
   - Layout shift causes identification
   - Image and font loading optimization
   - Dynamic content insertion
   - Animation and transition impact

4. **Additional Metrics:**
   - First Contentful Paint (FCP)
   - Time to Interactive (TTI)
   - Total Blocking Time (TBT)
   - Speed Index

**Analysis Commands:**
```bash
# Lighthouse audit
npx lighthouse http://localhost:4200 --only-categories=performance

# Web Vitals measurement
npm install web-vitals
# Add to main.ts for production monitoring

# Real User Monitoring
# Implement RUM for production metrics
```

---

## **Phase 4: Network Performance Analysis**

**Directive:** Analyze network performance and data fetching strategies.

**Network Analysis Areas:**

1. **API Performance:**
   - Request/response times
   - Data payload optimization
   - Caching strategy effectiveness
   - Error handling and retry patterns

2. **Resource Loading:**
   - Critical resource prioritization
   - Lazy loading implementation
   - Preloading strategies
   - CDN optimization

3. **Data Fetching Patterns:**
   - Observable subscription management
   - Request deduplication
   - Pagination and infinite scrolling
   - Real-time data updates

4. **Caching Strategies:**
   - HTTP caching headers
   - Service worker implementation
   - Local storage usage
   - Memory caching patterns

**Analysis Commands:**
```bash
# Network analysis
# Use Chrome DevTools Network tab
# Analyze request waterfall

# API performance
# Monitor API response times
# Check for unnecessary requests

# Caching analysis
# Verify cache headers
# Test offline functionality
```

---

## **Phase 5: Asset Optimization Analysis**

**Directive:** Analyze asset optimization opportunities and implementation.

**Asset Analysis Areas:**

1. **Image Optimization:**
   - Image format and compression
   - Responsive image implementation
   - Lazy loading effectiveness
   - WebP/AVIF format usage

2. **Font Optimization:**
   - Font loading strategies
   - Font subsetting and optimization
   - Preloading critical fonts
   - Fallback font implementation

3. **CSS Optimization:**
   - Critical CSS extraction
   - Unused CSS removal
   - CSS minification and compression
   - CSS-in-JS performance impact

4. **JavaScript Optimization:**
   - Code splitting effectiveness
   - Tree shaking implementation
   - Minification and compression
   - Source map optimization

**Analysis Commands:**
```bash
# Image optimization
npx imagemin src/assets/images/* --out-dir=dist/assets/images

# Font optimization
# Use tools like fonttools for subsetting

# CSS analysis
# Use tools like PurgeCSS for unused CSS removal
```

---

## **Phase 6: Angular-Specific Performance Analysis**

**Directive:** Analyze Angular-specific performance patterns and optimizations.

**Angular Analysis Areas:**

1. **Change Detection Optimization:**
   - OnPush strategy implementation
   - Change detection frequency
   - TrackBy function usage
   - Async pipe optimization

2. **Lazy Loading Analysis:**
   - Route-based lazy loading
   - Component lazy loading
   - Module lazy loading
   - Preloading strategies

3. **Service Performance:**
   - Service instantiation patterns
   - Dependency injection optimization
   - Observable subscription management
   - State management performance

4. **Template Optimization:**
   - Template compilation performance
   - Expression evaluation optimization
   - Pipe usage and caching
   - Structural directive performance

**Analysis Commands:**
```bash
# Angular build analysis
ng build --stats-json
npx webpack-bundle-analyzer dist/stats.json

# Change detection profiling
# Use Angular DevTools browser extension

# Service performance
# Profile service instantiation and usage
```

---

## **Phase 7: Performance Optimization Recommendations**

**Directive:** Synthesize analysis into actionable optimization recommendations.

**Recommendation Categories:**

1. **Immediate Optimizations:**
   - Quick wins with high impact
   - Low-effort, high-value changes
   - Critical performance issues
   - Emergency fixes

2. **Medium-term Optimizations:**
   - Architectural improvements
   - Code refactoring opportunities
   - Dependency optimization
   - Asset optimization

3. **Long-term Optimizations:**
   - Framework upgrades
   - Architecture redesign
   - Infrastructure improvements
   - Performance monitoring implementation

**Output Requirements:**
- **Performance Score**: Current performance rating
- **Critical Issues**: Issues requiring immediate attention
- **Optimization Roadmap**: Prioritized list of improvements
- **Implementation Guide**: Step-by-step optimization instructions
- **Success Metrics**: Measurable performance targets

---

## **Usage Examples**

- `/analyze-performance` - Analyze entire application performance
- `/analyze-performance @src/app/` - Analyze specific application directory
- `/analyze-performance @src/components/` - Analyze component performance
- `/analyze-performance --bundle-only` - Focus on bundle analysis only
- `/analyze-performance --runtime-only` - Focus on runtime performance only

## **Performance Analysis Features**

- **Bundle Analysis**: Comprehensive bundle size and composition analysis
- **Runtime Profiling**: Component and service performance analysis
- **Core Web Vitals**: User experience metrics measurement
- **Network Analysis**: API and resource loading optimization
- **Asset Optimization**: Image, font, and CSS optimization
- **Angular-Specific**: Framework-specific performance patterns
- **Actionable Recommendations**: Prioritized optimization roadmap

**Remember**: This command provides comprehensive performance analysis to identify optimization opportunities and improve user experience through systematic performance improvements.
