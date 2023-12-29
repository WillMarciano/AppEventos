import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, take } from 'rxjs';
import { Evento } from '../models/Evento';
import { environment } from '@environments/environment';

@Injectable()
// {  providedIn: 'root'}
export class EventoService {
  baseUrl = environment.apiURL + 'api/eventos';
  tokenHeader = new HttpHeaders({
    Authorization: `Bearer ${JSON.parse(localStorage.getItem('user')).token}`,
  });

  constructor(private http: HttpClient) {}

  public getEvento(): Observable<Evento[]> {
    return this.http
      .get<Evento[]>(this.baseUrl, { headers: this.tokenHeader })
      .pipe(take(1));
  }

  public getEventoByTema(tema: string): Observable<Evento[]> {
    return this.http
      .get<Evento[]>(`${this.baseUrl}/tema/${tema}`, {
        headers: this.tokenHeader,
      })
      .pipe(take(1));
  }

  public getEventoById(id: number): Observable<Evento> {
    return this.http
      .get<Evento>(`${this.baseUrl}/${id}`, { headers: this.tokenHeader })
      .pipe(take(1));
  }

  public post(evento: Evento): Observable<Evento> {
    return this.http
      .post<Evento>(this.baseUrl, evento, { headers: this.tokenHeader })
      .pipe(take(1));
  }

  public put(evento: Evento): Observable<Evento> {
    return this.http
      .put<Evento>(`${this.baseUrl}/${evento.id}`, evento, {
        headers: this.tokenHeader,
      })
      .pipe(take(1));
  }

  public deleteEvento(id: number): Observable<any> {
    return this.http
      .delete<string>(`${this.baseUrl}/${id}`, { headers: this.tokenHeader })
      .pipe(take(1));
  }

  postUpload(eventoId: number, file: File): Observable<Evento> {
    const fileToUpload = file[0] as File;
    const formData = new FormData();
    formData.append('file', fileToUpload);
    return this.http
      .post<Evento>(`${this.baseUrl}/upload-image/${eventoId}`, formData)
      .pipe(take(1));
  }
}
