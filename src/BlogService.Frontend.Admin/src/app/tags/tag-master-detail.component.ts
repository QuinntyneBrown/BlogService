import { TagAdd, TagDelete, TagEdit, tagActions } from "./tag.actions";
import { Tag } from "./tag.model";
import { TagService } from "./tag.service";

const template = require("./tag-master-detail.component.html");
const styles = require("./tag-master-detail.component.scss");

export class TagMasterDetailComponent extends HTMLElement {
    constructor(
        private _tagService: TagService = TagService.Instance	
	) {
        super();
        this.onTagAdd = this.onTagAdd.bind(this);
        this.onTagEdit = this.onTagEdit.bind(this);
        this.onTagDelete = this.onTagDelete.bind(this);
    }

    static get observedAttributes () {
        return [
            "tags"
        ];
    }

    connectedCallback() {
        this.innerHTML = `<style>${styles}</style> ${template}`;
        this._bind();
        this._setEventListeners();
    }

    private async _bind() {
        this.tags = await this._tagService.get();
        this.tagListElement.setAttribute("tags", JSON.stringify(this.tags));
    }

    private _setEventListeners() {
        this.addEventListener(tagActions.ADD, this.onTagAdd);
        this.addEventListener(tagActions.EDIT, this.onTagEdit);
        this.addEventListener(tagActions.DELETE, this.onTagDelete);
    }

    disconnectedCallback() {
        this.removeEventListener(tagActions.ADD, this.onTagAdd);
        this.removeEventListener(tagActions.EDIT, this.onTagEdit);
        this.removeEventListener(tagActions.DELETE, this.onTagDelete);
    }

    public async onTagAdd(e) {

        await this._tagService.add(e.detail.tag);
        this.tags = await this._tagService.get();
        
        this.tagListElement.setAttribute("tags", JSON.stringify(this.tags));
        this.tagEditElement.setAttribute("tag", JSON.stringify(new Tag()));
    }

    public onTagEdit(e) {
        this.tagEditElement.setAttribute("tag", JSON.stringify(e.detail.tag));
    }

    public async onTagDelete(e) {

        await this._tagService.remove(e.detail.tag.id);
        this.tags = await this._tagService.get();
        
        this.tagListElement.setAttribute("tags", JSON.stringify(this.tags));
        this.tagEditElement.setAttribute("tag", JSON.stringify(new Tag()));
    }

    attributeChangedCallback (name, oldValue, newValue) {
        switch (name) {
            case "tags":
                this.tags = JSON.parse(newValue);

                if (this.parentNode)
                    this.connectedCallback();

                break;
        }
    }

    public get value(): Array<Tag> { return this.tags; }

    private tags: Array<Tag> = [];
    public tag: Tag = <Tag>{};
    public get tagEditElement(): HTMLElement { return this.querySelector("ce-tag-edit-embed") as HTMLElement; }
    public get tagListElement(): HTMLElement { return this.querySelector("ce-tag-list-embed") as HTMLElement; }
}

customElements.define(`ce-tag-master-detail`,TagMasterDetailComponent);
