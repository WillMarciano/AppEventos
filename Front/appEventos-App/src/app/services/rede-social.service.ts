import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RedeSocial } from '@app/models/RedeSocial';
import { environment } from '@environments/environment';
import { Observable, take } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class RedeSocialService {
  baseURL = environment.apiURL + 'api/redesSociais';

  constructor(private http: HttpClient) {}

  public getRedesSociais(
    id: number,
    isEvento: boolean
  ): Observable<RedeSocial[]> {
    let URL = `${this.baseURL}/GetAll/${id}?isEvento=${isEvento}`;

    return this.http.get<RedeSocial[]>(URL).pipe(take(1));
  }

  public saveRedesSociais(
    id: number,
    isEvento: boolean,
    redesSociais: RedeSocial[],
  ): Observable<RedeSocial[]> {
    let URL = `${this.baseURL}/Salvar/${id}?isEvento=${isEvento}`;

    return this.http.put<RedeSocial[]>(URL, redesSociais).pipe(take(1));
  }

  /**
   *
   * @param origem Precisa passar a palavra 'palestrante' ou 'evento' - Escrito em minúsculo.
   * @param id Precisa passar o PalestranteId ou o EventoId dependendo da sua Origem.
   * @param redeSocialId Precia usar o id da Rede Social
   * @returns Observable<any> - Pois é o retorno da Rota.
   */
  public deleteRedeSocial(
    origem: string,
    id: number,
    redeSocialId: number
  ): Observable<any> {
    let URL =
      id === 0
        ? `${this.baseURL}/${origem}/${redeSocialId}`
        : `${this.baseURL}/${origem}/${id}/${redeSocialId}`;

    return this.http.delete(URL).pipe(take(1));
  }
}
