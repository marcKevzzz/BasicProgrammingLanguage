# Documentation interface

Use the supplied Tailwind documentation screenshot for monochrome light/dark surfaces and hierarchy, and the full-width Tailwind documentation layout for the application shell. This is an interactive program library, so task controls remain prominent.

- White light canvas; near-black zinc dark canvas. Neutral text, controls, borders, and active navigation. Reserve semantic colors for errors and meaningful result states.
- A sticky full-width navbar contains the program logo, global search, and theme switch. The shell fills the screen, with matching 24px gaps between sidebar and main pane and at the right edge. Keep the reading column centered inside the full-width main pane.
- An independently scrolling sidebar groups every program into native expandable categories. Start categories collapsed until the user opens them; allow one category open at a time and persist that choice across navigation and refresh; mark the current program when its category is expanded. Keep dropdown link spacing compact, with 44px touch targets on mobile. On mobile, use a drawer with Escape dismissal, focus containment, and an inert background.
- Use locally hosted Inter Variable with Arial/system fallbacks and the cv11 alternate, compact sidebar text, clear headings, and readable descriptions. Keep forms and tables consistent across programs. Do not add marketing filler or decorative metrics.
- Global search uses a native dialog, Ctrl/Cmd+K, keyboard result navigation, and explicit empty/results feedback.
- Theme preference applies before styles load. The user-triggered theme transition expands a circle from the theme button across the entire viewport, including the navbar, sidebar, and outer gutters, using the root View Transition snapshot. Use immediate changes under reduced motion or when the API is unavailable. Interrupted transitions must not lose the newest preference.
- Retain readable focus outlines and system-native controls. At small widths hide secondary metadata, preserve labels, and scroll wide tables inside their own container.

- At desktop widths, the diagonal slash pattern fills the 24px sidebar-to-main gap and the matching right gutter. Keep the main panel itself plain. Their strokes use the theme border token; hide the rails on mobile to preserve content width.

- Keep the interface restrained: plain program icons, regular-weight list titles, one clear active filter, and no repeated per-row category labels, navbar index link, or branded footer. Preserve the approved introduction, command example, and side rails.

- Use solid black/white active filter pills, compact 32–36px desktop controls, and 44px touch targets. Space dropdown program links with a small grid gap; use a subtle neutral fill, leading rule, and rounded trailing corners for the current link.

- Use native page scrolling so the main scrollbar is at the far-right screen edge and scrolling over the content gaps works. The navbar and sidebar remain sticky; fix the patterned gap and right gutter below the navbar independently of content. The sidebar scrolls separately when needed. Mobile keeps normal page scrolling and the navigation drawer.

- Give sidebar links and category headings neutral hover/focus feedback. Program-row arrows shift 3px up and right on hover or keyboard focus, except under reduced motion; keep 12px of right padding.

- Remember How it works and C# code disclosure states independently per program; unopened programs start collapsed. Use shared monochrome table styles and keep wide collection tables horizontally scrollable.
