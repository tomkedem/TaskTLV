// src/app/models/product.model.ts
// Interface representing a Product entity
export interface Product {
    productId: number;
    productName: string;
    inStock: boolean;    
    dateAdded: Date;
    arrivalDate: Date;
}
