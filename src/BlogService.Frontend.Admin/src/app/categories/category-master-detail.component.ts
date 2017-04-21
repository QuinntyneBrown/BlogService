import { CategoryAdd, CategoryDelete, CategoryEdit, categoryActions } from "./category.actions";
import { Category } from "./category.model";
import { CategoryService } from "./category.service";

const template = require("./category-master-detail.component.html");
const styles = require("./category-master-detail.component.scss");

export class CategoryMasterDetailComponent extends HTMLElement {
    constructor(
        private _categoryService: CategoryService = CategoryService.Instance	
	) {
        super();
        this.onCategoryAdd = this.onCategoryAdd.bind(this);
        this.onCategoryEdit = this.onCategoryEdit.bind(this);
        this.onCategoryDelete = this.onCategoryDelete.bind(this);
    }

    static get observedAttributes () {
        return [
            "categorys"
        ];
    }

    connectedCallback() {
        this.innerHTML = `<style>${styles}</style> ${template}`;
        this._bind();
        this._setEventListeners();
    }

    private async _bind() {
        this.categorys = await this._categoryService.get();
        this.categoryListElement.setAttribute("categorys", JSON.stringify(this.categorys));
    }

    private _setEventListeners() {
        this.addEventListener(categoryActions.ADD, this.onCategoryAdd);
        this.addEventListener(categoryActions.EDIT, this.onCategoryEdit);
        this.addEventListener(categoryActions.DELETE, this.onCategoryDelete);
    }

    disconnectedCallback() {
        this.removeEventListener(categoryActions.ADD, this.onCategoryAdd);
        this.removeEventListener(categoryActions.EDIT, this.onCategoryEdit);
        this.removeEventListener(categoryActions.DELETE, this.onCategoryDelete);
    }

    public async onCategoryAdd(e) {

        await this._categoryService.add(e.detail.category);
        this.categorys = await this._categoryService.get();
        
        this.categoryListElement.setAttribute("categorys", JSON.stringify(this.categorys));
        this.categoryEditElement.setAttribute("category", JSON.stringify(new Category()));
    }

    public onCategoryEdit(e) {
        this.categoryEditElement.setAttribute("category", JSON.stringify(e.detail.category));
    }

    public async onCategoryDelete(e) {

        await this._categoryService.remove(e.detail.category.id);
        this.categorys = await this._categoryService.get();
        
        this.categoryListElement.setAttribute("categorys", JSON.stringify(this.categorys));
        this.categoryEditElement.setAttribute("category", JSON.stringify(new Category()));
    }

    attributeChangedCallback (name, oldValue, newValue) {
        switch (name) {
            case "categorys":
                this.categorys = JSON.parse(newValue);

                if (this.parentNode)
                    this.connectedCallback();

                break;
        }
    }

    public get value(): Array<Category> { return this.categorys; }

    private categorys: Array<Category> = [];
    public category: Category = <Category>{};
    public get categoryEditElement(): HTMLElement { return this.querySelector("ce-category-edit-embed") as HTMLElement; }
    public get categoryListElement(): HTMLElement { return this.querySelector("ce-category-list-embed") as HTMLElement; }
}

customElements.define(`ce-category-master-detail`,CategoryMasterDetailComponent);
