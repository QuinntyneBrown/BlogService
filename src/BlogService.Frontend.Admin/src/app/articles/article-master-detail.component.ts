import { ArticleAdd, ArticleDelete, ArticleEdit, articleActions } from "./article.actions";
import { Article } from "./article.model";
import { ArticleService } from "./article.service";

const template = require("./article-master-detail.component.html");
const styles = require("./article-master-detail.component.scss");

export class ArticleMasterDetailComponent extends HTMLElement {
    constructor(
        private _articleService: ArticleService = ArticleService.Instance	
	) {
        super();
        this.onArticleAdd = this.onArticleAdd.bind(this);
        this.onArticleEdit = this.onArticleEdit.bind(this);
        this.onArticleDelete = this.onArticleDelete.bind(this);
    }

    static get observedAttributes () {
        return [
            "articles"
        ];
    }

    connectedCallback() {
        this.innerHTML = `<style>${styles}</style> ${template}`;
        this._bind();
        this._setEventListeners();
    }

    private async _bind() {
        this.articles = await this._articleService.get();
        this.articleListElement.setAttribute("articles", JSON.stringify(this.articles));
    }

    private _setEventListeners() {
        this.addEventListener(articleActions.ADD, this.onArticleAdd);
        this.addEventListener(articleActions.EDIT, this.onArticleEdit);
        this.addEventListener(articleActions.DELETE, this.onArticleDelete);
    }

    disconnectedCallback() {
        this.removeEventListener(articleActions.ADD, this.onArticleAdd);
        this.removeEventListener(articleActions.EDIT, this.onArticleEdit);
        this.removeEventListener(articleActions.DELETE, this.onArticleDelete);
    }

    public async onArticleAdd(e) {

        await this._articleService.add(e.detail.article);
        this.articles = await this._articleService.get();
        
        this.articleListElement.setAttribute("articles", JSON.stringify(this.articles));
        this.articleEditElement.setAttribute("article", JSON.stringify(new Article()));
    }

    public onArticleEdit(e) {
        this.articleEditElement.setAttribute("article", JSON.stringify(e.detail.article));
    }

    public async onArticleDelete(e) {

        await this._articleService.remove(e.detail.article.id);
        this.articles = await this._articleService.get();
        
        this.articleListElement.setAttribute("articles", JSON.stringify(this.articles));
        this.articleEditElement.setAttribute("article", JSON.stringify(new Article()));
    }

    attributeChangedCallback (name, oldValue, newValue) {
        switch (name) {
            case "articles":
                this.articles = JSON.parse(newValue);

                if (this.parentNode)
                    this.connectedCallback();

                break;
        }
    }

    public get value(): Array<Article> { return this.articles; }

    private articles: Array<Article> = [];
    public article: Article = <Article>{};
    public get articleEditElement(): HTMLElement { return this.querySelector("ce-article-edit-embed") as HTMLElement; }
    public get articleListElement(): HTMLElement { return this.querySelector("ce-article-list-embed") as HTMLElement; }
}

customElements.define(`ce-article-master-detail`,ArticleMasterDetailComponent);
