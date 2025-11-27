# BackendUi

This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 20.2.2.

## Development server

To start a local development server, run:

```bash
ng serve
```

Once the server is running, open your browser and navigate to `http://localhost:4200/`. The application will automatically reload whenever you modify any of the source files.

## Code scaffolding

Angular CLI includes powerful code scaffolding tools. To generate a new component, run:

```bash
ng generate component component-name
```

For a complete list of available schematics (such as `components`, `directives`, or `pipes`), run:

```bash
ng generate --help
```

## Building

To build the project run:

```bash
ng build
```

This will compile your project and store the build artifacts in the `dist/` directory. By default, the production build optimizes your application for performance and speed.

## Running unit tests

To execute unit tests with the [Karma](https://karma-runner.github.io) test runner, use the following command:

```bash
ng test
```

## Running end-to-end tests

For end-to-end (e2e) testing, run:

```bash
ng e2e
```

Angular CLI does not come with an end-to-end testing framework by default. You can choose one that suits your needs.

## PrimeNG & Tailwind setup

- The backend UI uses [PrimeNG](https://www.primefaces.org/primeng/) for rich UI components. Install the dependencies once with `npm install primeng primeicons primeflex` and import the modules you need in the corresponding feature modules or standalone components.
- Tailwind CSS powers the utility-based styling. The project ships with `tailwind.config.js` and `postcss.config.js` plus `@tailwind base/components/utilities` in `src/styles.css`. After installing `tailwindcss` (and its PostCSS/autoprefixer peer dependencies), any component’s template styles may use Tailwind classes immediately.
- Both libraries are bundled through the Angular CLI, so running `ng serve` or `ng build` will include the PrimeNG themes and the generated Tailwind utilities automatically.

## Additional Resources

For more information on using the Angular CLI, including detailed command references, visit the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.
