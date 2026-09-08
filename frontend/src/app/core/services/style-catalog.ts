import { Service, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

export interface StyleOption {
  id: number;
  name: string;
  description: string;
}

@Service()
export class StyleCatalog {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  getHairStyles() {
    return this.http.get<StyleOption[]>(`${this.apiUrl}/hair-styles`);
  }

  getBeardStyles() {
    return this.http.get<StyleOption[]>(`${this.apiUrl}/beard-styles`);
  }
}