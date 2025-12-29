interface MovieSearchPayload {
  movieId: number;
  includeFallback?: boolean;
}

type InteractiveSearchPayload = MovieSearchPayload;

export default InteractiveSearchPayload;
