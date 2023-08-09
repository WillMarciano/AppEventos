import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss'],
})
export class EventosComponent implements OnInit {
  public events: any = [];
  public filteredEvents: any = [];
  widthImg: number = 150;
  marginImg: number = 2;
  showImg: boolean = true;
  private _filterList: string = '';

  public get filterList(): string {
    return this._filterList;
  }

  public set filterList(value: string) {
    this._filterList = value;
    this.filteredEvents = this.filterList
      ? this.filterEvents(this.filterList)
      : this.events;
  }

  filterEvents(filterFor: string): any {
    filterFor = filterFor.toLocaleLowerCase();
    return this.events.filter(
      (evento: { tema: string; local: string }) =>
        evento.tema.toLocaleLowerCase().indexOf(filterFor) !== -1 ||
        evento.local.toLocaleLowerCase().indexOf(filterFor) !== -1
    );
  }

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getEventos();
  }

  alterImage() {
    this.showImg = !this.showImg;
  }

  public getEventos(): void {
    this.http.get('https://localhost:5001/api/eventos').subscribe(
      (response) => {
        this.events = response;
        this.filteredEvents = this.events;
      },
      (error) => console.log(error)
    );
  }
}
