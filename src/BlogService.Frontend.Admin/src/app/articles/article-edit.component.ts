import { Article } from "./article.model";
import { ArticleService } from "./article.service";
import { AuthorService } from "../authors";
import { TagService } from "../tags";
import { EditorComponent, Modal } from "../shared";
import { Router } from "../router";

const template = require("./article-edit.component.html");
const styles = require("./article-edit.component.scss");

export class ArticleEditComponent extends HTMLElement {
    constructor(
        private _articleService: ArticleService = ArticleService.Instance,
        private _authorService: AuthorService = AuthorService.Instance,
        private _modal: Modal = Modal.Instance,
        private _tagService: TagService = TagService.Instance,
        private _router: Router = Router.Instance
    ) {
        super();
        this.saveArticle = this.saveArticle.bind(this);
        this.onDelete = this.onDelete.bind(this);
    }

    static get observedAttributes() {
        return ["article-id"];
    }
    
    connectedCallback() {        
        this.innerHTML = `<style>${styles}</style> ${template}`; 
        this._bind();
        this._setEventListeners();
    }

    private async _bind() {
        this.titleElement.textContent = "Create Article";
        this.htmlContentEditor = new EditorComponent(this.htmlContentElement);

        rome(this.articlePublishedElement);

        let promises: Array<any> = [this._authorService.get(), this._tagService.get()];

        if (this.articleId) {
            promises.push(this._articleService.getById(this.articleId));
        }
        
        const results: Array<any> = await Promise.all(promises);
        const authors = results[0];
        const tags = results[1];

        for (let i = 0; i < authors.length; i++) {
            let option = document.createElement("option");
            option.textContent = `${authors[i].firstname} ${authors[i].lastname}`;
            option.value = authors[i].id;
            this.selectElement.appendChild(option);
        }

        if (this.articleId) {            
            const article: Article = results[2];           
            this.isPublishedElement.checked = article.isPublished;
            this.articleTitleInputElement.value = article.title;
            this.featuredImageUrlElement.value = article.featuredImageUrl;
            this.htmlContentEditor.setHTML(article.htmlContent); 
            this.articlePublishedElement.value = article.published;
            
            
            this.titleElement.textContent = "Edit Article";
        } else {
            this.deleteButtonElement.style.display = "none";
        }        
    }

    private _setEventListeners() {
        this.saveButtonElement.addEventListener("click", this.saveArticle);
        this.deleteButtonElement.addEventListener("click", this.onDelete);
    }

    public async saveArticle() {
        
        const article = {
            id: this.articleId,
            authorId: this.selectElement.value,
            title: this.articleTitleInputElement.value,
            featuredImageUrl: this.featuredImageUrlElement.value,
            htmlContent: this.htmlContentEditor.text,
            isPublished: this.isPublishedCheckedElement != null,
            published: this.articlePublishedElement.value
        } as Article;
        
        await this._articleService.add(article);
        this._router.navigate(["article", "list"]);
    }

    public async onDelete() {
        await this._articleService.remove({ id: this.articleId });
        this._router.navigate(["article", "list"]);
    }

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case "article-id":
                this.articleId = newValue;
				break;
        }        
    }

    public articleId: number;

    public published: string;

    public htmlContentEditor: EditorComponent;

    public get titleElement(): HTMLElement { return this.querySelector("h2") as HTMLInputElement; }

    public get featuredImageUrlElement(): HTMLInputElement { return this.querySelector(".article-featured-image-url") as HTMLInputElement; }

    public get htmlContentElement(): HTMLElement { return this.querySelector(".article-html-content") as HTMLElement; }

    public get saveButtonElement(): HTMLButtonElement { return this.querySelector(".save-button") as HTMLButtonElement; };

    public get deleteButtonElement(): HTMLButtonElement { return this.querySelector(".delete-button") as HTMLButtonElement; };

    public get articleTitleInputElement(): HTMLInputElement { return this.querySelector(".article-title") as HTMLInputElement; }

    public get articlePublishedElement(): HTMLInputElement { return this.querySelector(".article-published") as HTMLInputElement; }

    public get selectElement(): HTMLSelectElement { return this.querySelector("select") as HTMLSelectElement; }
    
    public get isPublishedCheckedElement(): HTMLInputElement { return this.querySelector(".article-is-published:checked") as HTMLInputElement; }

    public get isPublishedElement(): HTMLInputElement { return this.querySelector(".article-is-published") as HTMLInputElement; }
}

customElements.define(`ce-article-edit`,ArticleEditComponent);