import { Category } from "./category.model";
import { EditorComponent } from "../shared";
import {  CategoryDelete, CategoryEdit, CategoryAdd } from "./category.actions";

const template = require("./category-edit-embed.component.html");
const styles = require("./category-edit-embed.component.scss");

export class CategoryEditEmbedComponent extends HTMLElement {
    constructor() {
        super();
        this.onSave = this.onSave.bind(this);
        this.onDelete = this.onDelete.bind(this);
        this.onCreate = this.onCreate.bind(this);
    }

    static get observedAttributes() {
        return [
            "category",
            "category-id"
        ];
    }
    
    connectedCallback() {        
        this.innerHTML = `<style>${styles}</style> ${template}`; 
        this._bind();
        this._setEventListeners();
    }
    
    private async _bind() {
        this._titleElement.textContent = this.category ? "Edit Category": "Create Category";

        if (this.category) {                
            this._nameInputElement.value = this.category.name;  
        } else {
            this._deleteButtonElement.style.display = "none";
        }     
    }

    private _setEventListeners() {
        this._saveButtonElement.addEventListener("click", this.onSave);
        this._deleteButtonElement.addEventListener("click", this.onDelete);
        this._createButtonElement.addEventListener("click", this.onCreate);
    }

    private disconnectedCallback() {
        this._saveButtonElement.removeEventListener("click", this.onSave);
        this._deleteButtonElement.removeEventListener("click", this.onDelete);
        this._createButtonElement.removeEventListener("click", this.onCreate);
    }

    public onSave() {
        const category = {
            id: this.category != null ? this.category.id : null,
            name: this._nameInputElement.value
        } as Category;
        
        this.dispatchEvent(new CategoryAdd(category));            
    }

    public onCreate() {        
        this.dispatchEvent(new CategoryEdit(new Category()));            
    }

    public onDelete() {        
        const category = {
            id: this.category != null ? this.category.id : null,
            name: this._nameInputElement.value
        } as Category;

        this.dispatchEvent(new CategoryDelete(category));         
    }

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case "category-id":
                this.categoryId = newValue;
                break;
            case "category":
                this.category = JSON.parse(newValue);
                if (this.parentNode) {
                    this.categoryId = this.category.id;
                    this._nameInputElement.value = this.category.name != undefined ? this.category.name : "";
                    this._titleElement.textContent = this.categoryId ? "Edit Category" : "Create Category";
                }
                break;
        }           
    }

    public categoryId: any;
    
	public category: Category;
    
    private get _createButtonElement(): HTMLElement { return this.querySelector(".category-create") as HTMLElement; }
    
	private get _titleElement(): HTMLElement { return this.querySelector("h2") as HTMLElement; }
    
	private get _saveButtonElement(): HTMLElement { return this.querySelector(".save-button") as HTMLElement };
    
	private get _deleteButtonElement(): HTMLElement { return this.querySelector(".delete-button") as HTMLElement };
    
	private get _nameInputElement(): HTMLInputElement { return this.querySelector(".category-name") as HTMLInputElement;}
}

customElements.define(`ce-category-edit-embed`,CategoryEditEmbedComponent);
