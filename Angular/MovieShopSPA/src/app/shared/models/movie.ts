import {Genre} from "./genre";
import {Cast} from "./cast";

export interface Movie {
    id: number;
    title: string;
    posterUrl: string;
    releaseDate: string;
    backdropUrl: string;
    rating?: any;
    overview: string;
    tagline: string;
    budget: number;
    revenue: number;
    imdbUrl: string;
    tmdbUrl: string;
    createdDate?: any;
    originalLanguage?: any;
    runTime: number;
    price: number;
    favoritesCount: number;
    genres: Genre[];
    casts: Cast[];
}