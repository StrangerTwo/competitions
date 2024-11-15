import "./style.css";

import { hydrate } from "preact";
import render from "preact-render-to-string/jsx";

import { App } from "./app";

if (typeof window !== "undefined") {
    hydrate(<App />, document.querySelector("#app")!);
}

export async function prerender() {
    const html = render(<App />, {}, { pretty: true });

    return {
        html,
    };
}
