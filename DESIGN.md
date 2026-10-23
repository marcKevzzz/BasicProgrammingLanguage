# Documentation interface

Use the supplied Tailwind documentation screenshot for monochrome light/dark surfaces and hierarchy, and the Bootstrap documentation screenshot for centered page width and outer gutters. This is an interactive program library, so task controls remain prominent.

- White light canvas; near-black zinc dark canvas. Neutral text, controls, borders, and active navigation. Reserve semantic colors for errors and meaningful result states.
- A sticky full-width navbar contains the program logo, global search, and theme switch. A centered maximum-width container keeps generous desktop gutters.
- A sticky, independently scrolling sidebar groups every program into native expandable categories. Start every category collapsed on page load; mark the current program when its category is expanded. Keep dropdown link spacing compact, with 44px touch targets on mobile. On mobile, use a drawer with Escape dismissal, focus containment, and an inert background.
- Use locally hosted Inter Variable with Arial/system fallbacks and the cv11 alternate, compact sidebar text, clear headings, and readable descriptions. Keep forms and tables consistent across programs. Do not add marketing filler or decorative metrics.
- Global search uses a native dialog, Ctrl/Cmd+K, keyboard result navigation, and explicit empty/results feedback.
- Theme preference applies before styles load. The user-triggered theme transition expands a circle from the theme button across the entire viewport, including the navbar, sidebar, and outer gutters, using the root View Transition snapshot. Use immediate changes under reduced motion or when the API is unavailable. Interrupted transitions must not lose the newest preference.
- Retain readable focus outlines and system-native controls. At small widths hide secondary metadata, preserve labels, and scroll wide tables inside their own container.

- At desktop widths, subtle 18px diagonal hatched rails frame the main reading area, while the same pattern fills both outer page gutters all the way to the screen edges. Their strokes use the theme border token; hide the rails on mobile to preserve content width.

- Keep the interface restrained: plain program icons, regular-weight list titles, one clear active filter, and no repeated per-row category labels, navbar index link, or branded footer. Preserve the approved introduction, command example, and side rails.

- Use solid black/white active filter pills, compact 32–36px desktop controls, and 44px touch targets. Space dropdown program links with a small grid gap; use a subtle neutral fill, leading rule, and rounded trailing corners for the current link.
