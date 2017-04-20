export class Article { 

    public id:any;
    
    public name:string;

    public static fromJSON(data: { name:string }): Article {

        let article = new Article();

        article.name = data.name;

        return article;
    }
}
