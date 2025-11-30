# Performance Optimization Protocol

## Overview
Execute comprehensive performance optimization for Angular applications, focusing on bundle optimization, runtime performance, Core Web Vitals, and user experience improvements.

## Mission Briefing: Performance Optimization Protocol

**OPTIMIZATION COMMAND DETECTION:** Before proceeding with optimization, check if the provided text contains any `/analyze*` command (e.g., `/analyze-code`, `/analyze-project`, `/analyze-bug-*`, etc.). If found, skip this command's optimization and execute the detected command instead to avoid duplicate analysis.

You will now execute comprehensive performance optimization using the **AUTONOMOUS PRINCIPAL ENGINEER - OPERATIONAL DOCTRINE.** This optimization follows the Phase 0 reconnaissance principles with specialized focus on performance improvement and optimization.

---

## **Phase 0: Performance Optimization Reconnaissance & Mental Modeling (Read-Only)**

**Directive:** Perform non-destructive analysis of current performance state to build a complete understanding of optimization opportunities.

**Performance Optimization Scope:**
- **Bundle Optimization**: Bundle size reduction and code splitting
- **Runtime Optimization**: Component performance and change detection
- **Core Web Vitals**: LCP, FID, CLS optimization
- **Asset Optimization**: Images, fonts, CSS, JavaScript optimization
- **Network Optimization**: API calls, caching, and data fetching
- **Angular-Specific**: Framework-specific performance patterns

**Constraints:**
- **No mutations are permitted during this phase**
- **Focus solely on understanding current performance state**
- **Identify optimization opportunities without implementing changes**

---

## **Phase 1: Bundle Optimization**

**Directive:** Optimize bundle size and composition for better performance.

**Bundle Optimization Areas:**

1. **Code Splitting:**
   - Route-based code splitting
   - Component lazy loading
   - Library code splitting
   - Dynamic import optimization

2. **Tree Shaking:**
   - Unused code elimination
   - Dead code removal
   - Import optimization
   - Side effect analysis

3. **Bundle Composition:**
   - Vendor bundle optimization
   - Common chunk extraction
   - Bundle size analysis
   - Dependency optimization

4. **Compression and Minification:**
   - JavaScript minification
   - CSS minification
   - HTML minification
   - Gzip compression

**Bundle Optimization Commands:**
```bash
# Bundle analysis
ng build --stats-json
npx webpack-bundle-analyzer dist/stats.json

# Bundle optimization
ng build --prod --aot --build-optimizer
ng build --prod --aot --build-optimizer --vendor-chunk

# Code splitting
ng build --prod --aot --build-optimizer --named-chunks
```

**Bundle Optimization Patterns:**
```typescript
// Lazy loading components
const routes: Routes = [
  {
    path: 'dashboard',
    loadComponent: () => import('./dashboard/dashboard.component').then(m => m.DashboardComponent)
  },
  {
    path: 'profile',
    loadComponent: () => import('./profile/profile.component').then(m => m.ProfileComponent)
  }
];

// Dynamic imports
async loadFeature() {
  const { FeatureModule } = await import('./feature/feature.module');
  return FeatureModule;
}

// Tree shaking optimization
import { specificFunction } from 'large-library';
// Instead of: import * as library from 'large-library';
```

---

## **Phase 2: Runtime Performance Optimization**

**Directive:** Optimize runtime performance and component efficiency.

**Runtime Optimization Areas:**

1. **Change Detection Optimization:**
   - OnPush strategy implementation
   - TrackBy function optimization
   - Async pipe usage
   - Change detection frequency reduction

2. **Component Optimization:**
   - Component lifecycle optimization
   - Input/output property optimization
   - Template optimization
   - Event handling optimization

3. **Service Optimization:**
   - Service instantiation optimization
   - Observable subscription management
   - Caching implementation
   - Error handling optimization

4. **Memory Management:**
   - Memory leak prevention
   - Subscription cleanup
   - Object lifecycle management
   - Garbage collection optimization

**Runtime Optimization Patterns:**
```typescript
// OnPush change detection
@Component({
  selector: 'app-optimized-component',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `...`
})
export class OptimizedComponent {
  @Input() data: any;
  
  constructor(private cdr: ChangeDetectorRef) {}
  
  updateData(newData: any) {
    this.data = newData;
    this.cdr.markForCheck();
  }
}

// TrackBy function optimization
trackByFn(index: number, item: any): any {
  return item.id;
}

// Async pipe optimization
@Component({
  template: `
    <div *ngFor="let item of items$ | async; trackBy: trackByFn">
      {{ item.name }}
    </div>
  `
})
export class OptimizedListComponent {
  items$ = this.dataService.getItems();
  
  trackByFn(index: number, item: any): any {
    return item.id;
  }
}
```

**Runtime Optimization Commands:**
```bash
# Change detection profiling
# Use Angular DevTools browser extension
# Profile change detection cycles

# Memory profiling
# Use Chrome DevTools Memory tab
# Check for memory leaks and usage

# Performance profiling
# Use Chrome DevTools Performance tab
# Profile application performance
```

---

## **Phase 3: Core Web Vitals Optimization**

**Directive:** Optimize Core Web Vitals for better user experience.

**Core Web Vitals Optimization Areas:**

1. **Largest Contentful Paint (LCP) Optimization:**
   - Critical resource prioritization
   - Image optimization and lazy loading
   - Font loading optimization
   - Critical CSS extraction

2. **First Input Delay (FID) Optimization:**
   - JavaScript execution optimization
   - Main thread blocking reduction
   - Event handler optimization
   - Third-party script optimization

3. **Cumulative Layout Shift (CLS) Optimization:**
   - Image and font size specification
   - Dynamic content insertion optimization
   - Animation and transition optimization
   - Layout stability improvement

4. **Additional Metrics Optimization:**
   - First Contentful Paint (FCP)
   - Time to Interactive (TTI)
   - Total Blocking Time (TBT)
   - Speed Index

**Core Web Vitals Optimization Patterns:**
```typescript
// Critical CSS extraction
@Component({
  selector: 'app-critical',
  template: `
    <style>
      /* Critical CSS inline */
      .hero { background: #f0f0f0; }
    </style>
    <div class="hero">Critical content</div>
  `
})
export class CriticalComponent {}

// Image optimization
@Component({
  template: `
    <img 
      [src]="imageSrc" 
      [alt]="imageAlt"
      loading="lazy"
      [width]="imageWidth"
      [height]="imageHeight"
    >
  `
})
export class OptimizedImageComponent {
  @Input() imageSrc: string;
  @Input() imageAlt: string;
  @Input() imageWidth: number;
  @Input() imageHeight: number;
}

// Font loading optimization
@Component({
  template: `
    <link rel="preload" href="/assets/fonts/main.woff2" as="font" type="font/woff2" crossorigin>
    <style>
      @font-face {
        font-family: 'MainFont';
        src: url('/assets/fonts/main.woff2') format('woff2');
        font-display: swap;
      }
    </style>
  `
})
export class FontOptimizationComponent {}
```

**Core Web Vitals Optimization Commands:**
```bash
# Lighthouse audit
npx lighthouse http://localhost:4200 --only-categories=performance

# Web Vitals measurement
npm install web-vitals
# Add to main.ts for production monitoring

# Performance monitoring
# Implement RUM for production metrics
```

---

## **Phase 4: Asset Optimization**

**Directive:** Optimize images, fonts, CSS, and JavaScript assets.

**Asset Optimization Areas:**

1. **Image Optimization:**
   - Image format optimization (WebP, AVIF)
   - Image compression and resizing
   - Responsive image implementation
   - Lazy loading implementation

2. **Font Optimization:**
   - Font subsetting and optimization
   - Font loading strategies
   - Font display optimization
   - Fallback font implementation

3. **CSS Optimization:**
   - Critical CSS extraction
   - Unused CSS removal
   - CSS minification and compression
   - CSS-in-JS optimization

4. **JavaScript Optimization:**
   - Code splitting and lazy loading
   - Tree shaking and dead code elimination
   - Minification and compression
   - Source map optimization

**Asset Optimization Patterns:**
```typescript
// Responsive image implementation
@Component({
  template: `
    <picture>
      <source media="(min-width: 768px)" [srcset]="largeImageSrc">
      <source media="(min-width: 480px)" [srcset]="mediumImageSrc">
      <img [src]="smallImageSrc" [alt]="imageAlt" loading="lazy">
    </picture>
  `
})
export class ResponsiveImageComponent {
  @Input() largeImageSrc: string;
  @Input() mediumImageSrc: string;
  @Input() smallImageSrc: string;
  @Input() imageAlt: string;
}

// Font optimization
@Component({
  template: `
    <link rel="preload" href="/assets/fonts/main.woff2" as="font" type="font/woff2" crossorigin>
    <style>
      @font-face {
        font-family: 'MainFont';
        src: url('/assets/fonts/main.woff2') format('woff2');
        font-display: swap;
      }
      .optimized-text {
        font-family: 'MainFont', Arial, sans-serif;
      }
    </style>
  `
})
export class FontOptimizationComponent {}
```

**Asset Optimization Commands:**
```bash
# Image optimization
npx imagemin src/assets/images/* --out-dir=dist/assets/images

# Font optimization
# Use tools like fonttools for subsetting

# CSS optimization
npx purgecss --css dist/styles.css --content dist/index.html --output dist/optimized/

# JavaScript optimization
ng build --prod --aot --build-optimizer
```

---

## **Phase 5: Network Optimization**

**Directive:** Optimize network performance and API calls.

**Network Optimization Areas:**

1. **API Optimization:**
   - Request/response optimization
   - Caching implementation
   - Request deduplication
   - Error handling and retry logic

2. **Data Fetching Optimization:**
   - Pagination and infinite scrolling
   - Data prefetching and preloading
   - Real-time data optimization
   - Offline functionality

3. **Caching Optimization:**
   - HTTP caching headers
   - Service worker implementation
   - Local storage optimization
   - Memory caching patterns

4. **CDN and Content Delivery:**
   - CDN configuration
   - Asset delivery optimization
   - Geographic distribution
   - Cache invalidation strategies

**Network Optimization Patterns:**
```typescript
// API caching service
@Injectable()
export class CachedApiService {
  private cache = new Map<string, any>();
  private cacheTimeout = 5 * 60 * 1000; // 5 minutes

  get<T>(url: string): Observable<T> {
    const cached = this.cache.get(url);
    if (cached && Date.now() - cached.timestamp < this.cacheTimeout) {
      return of(cached.data);
    }

    return this.http.get<T>(url).pipe(
      tap(data => {
        this.cache.set(url, {
          data,
          timestamp: Date.now()
        });
      }),
      shareReplay(1)
    );
  }
}

// Request deduplication
@Injectable()
export class DeduplicatedApiService {
  private pendingRequests = new Map<string, Observable<any>>();

  get<T>(url: string): Observable<T> {
    if (this.pendingRequests.has(url)) {
      return this.pendingRequests.get(url);
    }

    const request = this.http.get<T>(url).pipe(
      finalize(() => this.pendingRequests.delete(url)),
      shareReplay(1)
    );

    this.pendingRequests.set(url, request);
    return request;
  }
}
```

**Network Optimization Commands:**
```bash
# Network analysis
# Use Chrome DevTools Network tab
# Analyze request/response timing

# API optimization
# Implement caching and deduplication
# Optimize request/response patterns

# CDN optimization
# Configure CDN for asset delivery
# Implement cache invalidation
```

---

## **Phase 6: Angular-Specific Optimization**

**Directive:** Optimize Angular-specific performance patterns.

**Angular Optimization Areas:**

1. **Component Optimization:**
   - Component lifecycle optimization
   - Input/output property optimization
   - Template optimization
   - Event handling optimization

2. **Service Optimization:**
   - Service instantiation optimization
   - Dependency injection optimization
   - Observable subscription management
   - State management optimization

3. **Routing Optimization:**
   - Lazy loading optimization
   - Route preloading strategies
   - Navigation optimization
   - Guard and resolver optimization

4. **Form Optimization:**
   - Reactive form optimization
   - Template-driven form optimization
   - Validation optimization
   - Form submission optimization

**Angular Optimization Patterns:**
```typescript
// Optimized component
@Component({
  selector: 'app-optimized',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div *ngFor="let item of items; trackBy: trackByFn">
      {{ item.name }}
    </div>
  `
})
export class OptimizedComponent {
  @Input() items: any[] = [];
  
  trackByFn(index: number, item: any): any {
    return item.id;
  }
}

// Optimized service
@Injectable({
  providedIn: 'root'
})
export class OptimizedService {
  private cache = new Map<string, any>();
  
  getData(id: string): Observable<any> {
    if (this.cache.has(id)) {
      return of(this.cache.get(id));
    }
    
    return this.http.get(`/api/data/${id}`).pipe(
      tap(data => this.cache.set(id, data)),
      shareReplay(1)
    );
  }
}
```

**Angular Optimization Commands:**
```bash
# Angular build optimization
ng build --prod --aot --build-optimizer --vendor-chunk

# Change detection optimization
# Use OnPush strategy for all components
# Implement trackBy functions for *ngFor

# Service optimization
# Use providedIn: 'root' for singleton services
# Implement proper subscription management
```

---

## **Phase 7: Performance Monitoring and Validation**

**Directive:** Monitor and validate performance optimizations.

**Performance Monitoring Areas:**

1. **Performance Metrics:**
   - Core Web Vitals monitoring
   - Bundle size monitoring
   - Runtime performance monitoring
   - User experience metrics

2. **Performance Testing:**
   - Performance regression testing
   - Load testing
   - Stress testing
   - User experience testing

3. **Performance Reporting:**
   - Performance dashboard
   - Performance alerts
   - Performance trends
   - Performance recommendations

4. **Performance Optimization:**
   - Continuous optimization
   - Performance budget enforcement
   - Performance best practices
   - Performance training

**Performance Monitoring Commands:**
```bash
# Performance monitoring
npm run performance:monitor
npm run performance:audit
npm run performance:report

# Performance testing
npm run test:performance
npm run test:load
npm run test:stress

# Performance validation
npm run validate:performance
npm run validate:web-vitals
```

---

## **Usage Examples**

- `/optimize` - Execute comprehensive performance optimization
- `/optimize --bundle-only` - Focus on bundle optimization only
- `/optimize --runtime-only` - Focus on runtime optimization only
- `/optimize --web-vitals-only` - Focus on Core Web Vitals optimization only
- `/optimize @src/components/` - Optimize specific component directory
- `/optimize --asset-only` - Focus on asset optimization only
- `/optimize --network-only` - Focus on network optimization only
- `/optimize --angular-only` - Focus on Angular-specific optimization only

## **Optimization Features**

- **Bundle Optimization**: Comprehensive bundle size reduction and code splitting
- **Runtime Optimization**: Component performance and change detection optimization
- **Core Web Vitals**: LCP, FID, CLS, and user experience optimization
- **Asset Optimization**: Images, fonts, CSS, and JavaScript optimization
- **Network Optimization**: API calls, caching, and data fetching optimization
- **Angular-Specific**: Framework-specific performance patterns and optimization
- **Performance Monitoring**: Continuous performance monitoring and validation
- **Actionable Recommendations**: Prioritized optimization roadmap and implementation guide

**Remember**: This command provides comprehensive performance optimization to improve application speed, user experience, and Core Web Vitals through systematic optimization strategies and best practices.
