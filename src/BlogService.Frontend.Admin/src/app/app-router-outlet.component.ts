import { RouterOutlet } from "./router";
import { AuthorizedRouteMiddleware } from "./users";

export class AppRouterOutletComponent extends RouterOutlet {
    constructor(el: any) {
        super(el);
    }

    connectedCallback() {
        this.setRoutes([
            { path: "/", name: "article-list", authRequired: true },
            { path: "/article/list", name: "article-list", authRequired: true },
            { path: "/article/edit/:articleId", name: "article-edit", authRequired: true },
            { path: "/article/create", name: "article-edit", authRequired: true },
            { path: "/register", name: "account-register" },
            { path: "/login", name: "login" },
            { path: "/error", name: "error" },
            { path: "*", name: "not-found" }
        ] as any);

        this.use(new AuthorizedRouteMiddleware());

        super.connectedCallback();
    }

}

customElements.define(`ce-app-router-oulet`, AppRouterOutletComponent);